using System;
using UnityEngine.Advertisements;

namespace MobileLandTemplate
{
    public class UnityAdsComponent : IAds
    {
        private string adsId;
        private string rewardId;

        public UnityAdsComponent(string gameId, string adsId, string rewardId)
        {
            Advertisement.Initialize(gameId);
            this.adsId = adsId;
            this.rewardId = rewardId;
        }

        public bool AdsReady
        {
            get
            {
                return Advertisement.IsReady(adsId);
            }
        }

        public bool RewardReady
        {
            get
            {
                return Advertisement.IsReady(rewardId);
            }
        }

        public void ShowAds()
        {
            Advertisement.Show(adsId);
        }

        public void ShowReward(Action callBackReward, Action callBackFail)
        {
            if (RewardReady)
            {
                var options = new ShowOptions();
                options.resultCallback = (result) =>
                {
                    if (result == ShowResult.Finished)
                    {
                        callBackReward.Invoke();
                    }
                    else
                    {
                        callBackFail.Invoke();
                    }
                };
                Advertisement.Show(rewardId, options);
            }
        }
    }
}