using System;

namespace MobileLandTemplate
{
    public class VungleAdsComponent : IAds
    {
        private string adsPlacement;
        private string rewardPlacement;

        public VungleAdsComponent(string appId, string adsPlacement, string rewardPlacement)
        {
            this.adsPlacement = adsPlacement;
            this.rewardPlacement = rewardPlacement;
            string[] placements = new string[] {
                adsPlacement,
                rewardPlacement
            };
            Vungle.onInitializeEvent += Vungle_OnInitializeEvent;
            Vungle.init(appId, placements);
        }

        void Vungle_OnInitializeEvent()
        {
            Vungle.onAdFinishedEvent += Vungle_OnAdFinishedEvent;
        }


        public bool AdsReady
        {
            get
            {
                return Vungle.isAdvertAvailable(adsPlacement);
            }
        }

        public bool RewardReady
        {
            get
            {
                return Vungle.isAdvertAvailable(rewardPlacement);
            }
        }

        public void ShowAds()
        {
            Vungle.loadAd(adsPlacement);
        }
        private Action rewardDone;
        private Action rewardFailed;

        public void ShowReward(Action callBackReward, Action callBackFailed)
        {
            if (!RewardReady)
            {
                callBackFailed.Invoke();
                return;
            }
            this.rewardDone = callBackReward;
            this.rewardFailed = callBackFailed;
            Vungle.loadAd(rewardPlacement);
        }

        void Vungle_OnAdFinishedEvent(string arg1, AdFinishedEventArgs arg2)
        {
            if (arg2.IsCompletedView)
            {
                rewardDone.Invoke();
            }
            else
            {
                rewardFailed.Invoke();
            }
        }

    }
}