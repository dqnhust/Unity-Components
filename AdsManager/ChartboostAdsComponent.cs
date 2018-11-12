using System;
using ChartboostSDK;

namespace VnzITComponent
{
    public class ChartboostAdsComponent : IAds
    {
        private readonly CBLocation location = CBLocation.HomeScreen;

        public ChartboostAdsComponent(string appId, string appSignature)
        {
            Chartboost.didInitialize += Chartboost_DidInitialize;
            Chartboost.didCompleteRewardedVideo += Chartboost_DidCompleteRewardedVideo;
            Chartboost.didCloseRewardedVideo += Chartboost_DidCloseRewardedVideo;
            Chartboost.CreateWithAppId(appId, appSignature);
        }

        private Action callBackReward;
        private Action callBackFail;
        private bool rewarded = false;

        void Chartboost_DidInitialize(bool inited)
        {
            Chartboost.setAutoCacheAds(true);
            Chartboost.cacheInterstitial(location);
        }

        void Chartboost_DidCompleteRewardedVideo(CBLocation arg1, int arg2)
        {
            rewarded = true;
        }

        void Chartboost_DidCloseRewardedVideo(CBLocation obj)
        {
            if (rewarded)
            {
                if (callBackReward != null)
                {
                    callBackReward.Invoke();
                }
            }
            else
            {
                if (callBackFail != null)
                {
                    callBackFail.Invoke();
                }
            }
        }

        public bool AdsReady
        {
            get
            {
                return Chartboost.hasInterstitial(location);
            }
        }

        public bool RewardReady
        {
            get
            {
                return Chartboost.hasRewardedVideo(location);
            }

        }

        public void ShowAds()
        {
            if (AdsReady)
                Chartboost.showInterstitial(location);
        }

        public void ShowReward(Action callBackReward, Action callBackFail)
        {
            if (!RewardReady)
            {
                callBackFail.Invoke();
                return;
            }
            this.callBackFail = callBackFail;
            this.callBackReward = callBackReward;
            rewarded = false;
            Chartboost.showRewardedVideo(location);
        }
    }
}