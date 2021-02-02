using UnityEngine;
using UnityEngine.Events;

namespace RateConfig
{
    [CreateAssetMenu(menuName = "GameData/Create ShareConfig", fileName = "ShareConfig", order = 0)]
    public class ShareConfig : ScriptableObject
    {
        [SerializeField] private string subject;
        [SerializeField] private UnityEvent onShared;

        public void Share()
        {
            new NativeShare()
                .SetSubject(subject).SetText(GetLink())
                .SetCallback((result, shareTarget) =>
                {
                    if (result == NativeShare.ShareResult.Shared)
                    {
                        onShared?.Invoke();
                    }

                    Debug.Log("Share result: " + result + ", selected app: " + shareTarget);
                })
                .Share();
        }

        private static string GetLink()
        {
            return
                $"https://play.google.com/store/apps/details?id={Application.identifier}&referrer=utm_source=in_app_share";
        }
    }
}