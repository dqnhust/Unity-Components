using UnityEngine;

namespace PopupRate
{
    public class TotalStarView : MonoBehaviour
    {
        [SerializeField] private GameObject[] stars;

        private int _currentStar = 1;

        public int CurrentStar => _currentStar;

        public void Show(int starCount)
        {
            _currentStar = starCount;
            for (int i = 0; i < stars.Length; i++)
            {
                var view = stars[i];
                view.SetActive(i < starCount);
            }
        }
    }
}