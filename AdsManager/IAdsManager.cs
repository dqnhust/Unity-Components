using System;

namespace AdsManager
{
    public interface IAdsManager
    {
        event Action OnRewardLoaded;
        event Action OnAdsClicked;
        void Init();
        void ShowInterstitial();
        void ShowBanner();
        void HideBanner();
        bool RewardVideoReady();
        void ShowReward(Action callBackRewardOpen, Action callBackRewardFailedToOpen, Action callBackRewardClose, Action callBackRewardCancel, Action callBackRewardSuccess);
    }
}