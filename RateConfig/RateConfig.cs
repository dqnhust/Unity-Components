using UnityEngine;
using UnityEngine.Events;

namespace RateConfig
{
    [CreateAssetMenu(menuName = "GameData/Create RateConfig", fileName = "RateConfig", order = 0)]
    public class RateConfig : ScriptableObject
    {
        [SerializeField] private UnityEvent onOpenRateLink;

        public void OpenRateLink()
        {
            Application.OpenURL(GetLink());
            onOpenRateLink?.Invoke();
        }

        private static string GetLink()
        {
            return
                $"https://play.google.com/store/apps/details?id={Application.identifier}&referrer=utm_source=in_app_share";
        }
    }
}