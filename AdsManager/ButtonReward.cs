using UnityEngine;
using UnityEngine.Events;

namespace AdsManager
{
    public class ButtonReward : MonoBehaviour
    {
        [SerializeField] private AdsManager adsManager;
        [SerializeField] private UnityEvent onRewardNotReady;
        [SerializeField] private UnityEvent onRewardReady;
        [SerializeField] private UnityEvent onRewardOpen;
        [SerializeField] private UnityEvent onRewardFailedOpen;
        [SerializeField] private UnityEvent onRewardClosed;
        [SerializeField] private UnityEvent onRewardSuccess;
        [SerializeField] private UnityEvent onRewardCancel;

        private void OnEnable()
        {
            CheckAndProcessRewardLoaded();
            adsManager.OnRewardLoaded += AdsManagerOnOnRewardLoaded;
        }

        private void OnDisable()
        {
            adsManager.OnRewardLoaded -= AdsManagerOnOnRewardLoaded;
        }

        private void AdsManagerOnOnRewardLoaded()
        {
            CheckAndProcessRewardLoaded();
        }

        private void CheckAndProcessRewardLoaded()
        {
            if (adsManager.RewardVideoReady())
            {
                onRewardReady?.Invoke();
            }
            else
            {
                onRewardNotReady?.Invoke();
            }
        }

        public void ShowReward()
        {
            adsManager.ShowReward(onRewardOpen.Invoke, onRewardFailedOpen.Invoke, onRewardClosed.Invoke,
                onRewardCancel.Invoke, onRewardSuccess.Invoke);
        }
    }
}