#pragma warning disable 0649
using System;
using System.Collections;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace Ads
{
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] private string bannerId;
        [SerializeField] private string interstitialId;
        [SerializeField] private string rewardedId;

        public void Init(bool enableBanner, bool enableInterstitial, bool enableRewarded)
        {
            MobileAds.Initialize(initStatus =>
            {
                
                Debug.Log("Admob Mobile Ads initialized, Status: " + initStatus);
                InvokeOnMainThread(() =>
                {
                    if (enableBanner)
                    {
                        RequestBanner();
                    }

                    if (enableInterstitial)
                    {
                        RequestAndLoadInterstitialAd();
                    }

                    if (enableRewarded)
                    {
                        RequestAndLoadRewardedAd();
                    }
                });
            });
        }

        // ReSharper disable once UnusedMember.Global
        public void ShowBanner()
        {
            _bannerView?.Show();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void HideBanner()
        {
            _bannerView?.Hide();
        }

        // ReSharper disable once UnusedMember.Global
        public void ShowInterstitial()
        {
            if (_interstitialAd != null && _interstitialAd.IsLoaded())
            {
                _interstitialAd.Show();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void ShowRewarded(Action callBackOpenReward, Action callBackDone, Action callBackFailed,
            Action callBackClose)
        {
            Debug.LogError("Call Show Reward");
            _actionFailedReward = callBackFailed;
            _actionDoneReward = callBackDone;
            _actionCloseReward = callBackClose;
            _actionRewardOpen = callBackOpenReward;
            if (_rewardedAd != null && _rewardedAd.IsLoaded())
            {
                _rewardedAd.Show();
            }
            else
            {
                Debug.LogError("RewardVideo not ready!");
                callBackFailed?.Invoke();
            }
        }

        #region Banner Setup

        private BannerView _bannerView;

        private AdRequest GetRequest()
        {
            return new AdRequest.Builder().AddTestDevice("597AE8AFFA7FF76120D8F02976C52C23").Build();
        }

        private void RequestBanner()
        {
            _bannerView?.Destroy();
            _bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
            _bannerView.LoadAd(GetRequest());
            HideBanner();
        }

        #endregion

        #region Interstitial Setup

        private InterstitialAd _interstitialAd;

        // ReSharper disable once MemberCanBePrivate.Global
        public void RequestAndLoadInterstitialAd()
        {
            _interstitialAd?.Destroy();
            _interstitialAd = new InterstitialAd(interstitialId);
            _interstitialAd.OnAdClosed += (sender, args) => RequestAndLoadInterstitialAd();
            _interstitialAd.OnAdFailedToLoad += (sender, args) =>
            {
                InvokeOnMainThread(() => InvokeDelay(15f, RequestAndLoadInterstitialAd));
            };
            _interstitialAd.LoadAd(GetRequest());
        }

        #endregion

        #region Rewarded Setup

        private RewardedAd _rewardedAd;

        private void RequestAndLoadRewardedAd()
        {
            // _gotRewarded = false;
            _rewardedAd = new RewardedAd(rewardedId);

            _rewardedAd.OnAdClosed += (sender, args) =>
            {
                InvokeOnMainThread(() => InvokeDelay(0.1f, () =>
                {
                    Debug.LogError("Ad Closed!");
                    _actionCloseReward?.Invoke();
                    RequestAndLoadRewardedAd();
                }));
            };
            _rewardedAd.OnUserEarnedReward += (sender, args) =>
            {
                InvokeOnMainThread(_actionDoneReward);
                // _gotRewarded = true;
                Debug.LogError("Got Reward!");
            };

            _rewardedAd.OnAdFailedToLoad += (sender, args) =>
            {
                Debug.LogError("Failed To Load!");
                InvokeOnMainThread(() => InvokeDelay(5f, RequestAndLoadRewardedAd));
            };

            _rewardedAd.OnAdFailedToShow += (sender, args) =>
            {
                Debug.LogError("Failed To Show!");
                InvokeOnMainThread(_actionFailedReward);
            };

            _rewardedAd.OnAdOpening += delegate
            {
                Debug.LogError("Ad Opening!");
                InvokeOnMainThread(_actionRewardOpen);
            };
            _rewardedAd.LoadAd(GetRequest());
        }

        private Action _actionDoneReward;
        private Action _actionFailedReward;
        private Action _actionCloseReward;
        private Action _actionRewardOpen;

        #endregion

        private void InvokeOnMainThread(Action method)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(method);
        }

        private void InvokeDelay(float delay, Action method)
        {
            StartCoroutine(IeInvokeDelay(delay, method));
        }

        private IEnumerator IeInvokeDelay(float delay, Action method)
        {
            yield return new WaitForSeconds(delay);
            method?.Invoke();
        }
    }
}