#pragma warning disable 0649
using DataManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PopupRate
{
    public class PopupRate : Popup.Popup
    {
        [SerializeField] private TotalStarView totalStarView;
        [SerializeField] private UnityEvent eventRate5Star;
        [SerializeField] private UnityEvent eventLessThan5Star;
        [SerializeField] private GameObject textPleaseSelect;
        [SerializeField] private Selectable buttonRate;

        private EventNumber<bool> Rated => DataStorage.GetData("Rated", new EventNumber<bool>(false));

        private void OnCallShowPopupRate()
        {
            if (Rated.Value)
            {
                return;
            }

            Open();
        }

        public void ClickRate()
        {
            if (totalStarView.CurrentStar >= 5)
            {
                Rated.Value = true;
                eventRate5Star?.Invoke();
            }
            else
            {
                eventLessThan5Star?.Invoke();
            }

            Close();
        }

        public void OnClickStar(int starCount)
        {
            totalStarView.Show(starCount);
            textPleaseSelect.SetActive(false);
            buttonRate.interactable = true;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            textPleaseSelect.SetActive(true);
            totalStarView.Show(0);
            buttonRate.interactable = false;
        }
    }
}