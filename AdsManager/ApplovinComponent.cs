using System;
using UnityEngine;

namespace VnzITComponent
{
    public class ApplovinComponent : IAds
    {
        private ApplovinListener listener;

        public ApplovinComponent()
        {
            AppLovin.InitializeSdk();
            AppLovin.PreloadInterstitial();
            AppLovin.LoadRewardedInterstitial();
            var g = new GameObject("ApplovinListener", typeof(ApplovinListener));
            listener = g.GetComponent<ApplovinListener>();
        }

        public bool AdsReady
        {
            get
            {
                return AppLovin.HasPreloadedInterstitial();
            }
        }

        public bool RewardReady
        {
            get
            {
                return AppLovin.IsIncentInterstitialReady();
            }
        }

        public void ShowAds()
        {
            if (AdsReady)
            {
                AppLovin.ShowInterstitial();
            }
        }

        public void ShowReward(Action callBackReward, Action callBackFail)
        {
            listener.Setup(callBackReward, callBackFail);
            AppLovin.ShowRewardedInterstitial();
        }

        public class ApplovinListener : MonoBehaviour
        {
            private Action done;
            private Action fail;
            private bool rewarded = false;

            public void Setup(Action callBackReward, Action callBackFailed)
            {
                rewarded = false;
                done = callBackReward;
                fail = callBackFailed;
            }

            private void onAppLovinEventReceived(string ev)
            {
                // The format would be "REWARDAPPROVEDINFO|AMOUNT|CURRENCY"
                if (ev.Contains("REWARDAPPROVEDINFO"))
                {
                    rewarded = true;
                }
                else if (ev.Contains("LOADEDREWARDED"))
                {
                    // A rewarded video was successfully loaded.
                }
                else if (ev.Contains("LOADREWARDEDFAILED"))
                {
                    // A rewarded video failed to load.
                }
                else if (ev.Contains("HIDDENREWARDED"))
                {
                    // A rewarded video has been closed.  Preload the next rewarded video.
                    AppLovin.LoadRewardedInterstitial();
                    if (rewarded)
                    {
                        done.Invoke();
                    }
                    else
                    {
                        fail.Invoke();
                    }
                }
            }
        }
    }
}