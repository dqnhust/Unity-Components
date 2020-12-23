#pragma warning disable 0649
using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
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
            MobileAds.Initialize(initStatus => { Debug.Log("Admob Mobile Ads initialized, Status: " + initStatus); });
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
        public void ShowRewarded(Action callBackOpenReward, Action callBackDone, Action callBackClose)
        {
            Debug.LogError("Call Show Reward");
            _actionDoneReward = callBackDone;
            _actionCloseReward = callBackClose;
            _actionRewardOpen = callBackOpenReward;
            if (_rewardedAd != null && _rewardedAd.IsLoaded())
            {
                _rewardedAd.Show();
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
                InvokeOnMainThread(() =>
                    InvokeDelay(15f, RequestAndLoadInterstitialAd)
                );
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
                    InvokeOnMainThread(_actionCloseReward);
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
                InvokeOnMainThread(() => InvokeDelay(5f, RequestAndLoadRewardedAd));
            };

            _rewardedAd.OnAdOpening += delegate(object sender, EventArgs args)
            {
                InvokeOnMainThread(_actionRewardOpen);
            };
            _rewardedAd.LoadAd(GetRequest());
        }

        private Action _actionDoneReward;
        private Action _actionCloseReward;
        private Action _actionRewardOpen;

        #endregion

        private readonly List<Action> _listMethodWillBeInvokeOnMainThread = new List<Action>();

        private void InvokeOnMainThread(Action method)
        {
            _listMethodWillBeInvokeOnMainThread.Add(method);
        }

        private void Update()
        {
            if (_listMethodWillBeInvokeOnMainThread.Count > 0)
            {
                foreach (var method in _listMethodWillBeInvokeOnMainThread)
                {
                    method?.Invoke();
                }

                _listMethodWillBeInvokeOnMainThread.Clear();
            }
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