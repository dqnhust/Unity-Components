using System;
using UnityEngine;

namespace Ui.Popup
{
    [CreateAssetMenu(menuName = "GameData/Create PopupEvent", fileName = "PopupEvent", order = 0)]
    public class PopupEvent : ScriptableObject
    {
        public event Action<GameObject> OnCallShowPopup;

        public void ShowPopup(GameObject popupPrefab)
        {
            OnCallShowPopup?.Invoke(popupPrefab);
        }
    }
}