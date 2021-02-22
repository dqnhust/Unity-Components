using System;
using System.Collections;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace AdsManager
{
    [CreateAssetMenu(menuName = "GameData/Create AdsManager", fileName = "AdsManager", order = 0)]
    public class AdsManager : ScriptableObject, IAdsManager
    {
        [SerializeField] private bool enableAds = true;
        [SerializeField] private string bannerId;
        [SerializeField] private string interstitialId;
        [SerializeField] private string rewardId;
        [SerializeField] private AdPosition bannerPosition;

        #region Implement Interface

        public event Action OnRewardLoaded;
        public event Action OnAdsClicked;

        public void Init()
        {
            if (!enableAds)
            {
                return;
            }
            MobileAds.Initialize(initStatus => { Debug.Log("Mobile Ads Initialized" + initStatus); });
            RequestBanner();
            RequestInterstitial();
            RequestReward();
        }


        public bool InterstitialReady()
        {
            return _interstitialAd != null && _interstitialAd.IsLoaded();
        }

        public void ShowInterstitial()
        {
            if (_interstitialAd.IsLoaded())
            {
                _interstitialAd.Show();
            }
        }

        public void ShowBanner()
        {
            if (_bannerShowing)
            {
                return;
            }

            _bannerView?.Show();
            _bannerShowing = true;
        }

        public void HideBanner()
        {
            if (!_bannerShowing)
            {
                return;
            }

            _bannerView?.Hide();
            _bannerShowing = false;
        }

        public bool RewardVideoReady()
        {
            return _rewardedAd.IsLoaded();
        }

        public void ShowReward(Action callBackRewardOpen, Action callBackRewardFailedToOpen, Action callBackRewardClose,
            Action callBackRewardCancel, Action callBackRewardSuccess)
        {
            if (!_rewardedAd.IsLoaded())
            {
                callBackRewardFailedToOpen?.Invoke();
                return;
            }

            _callBackRewardOpen = callBackRewardOpen;
            _callBackRewardFailedToOpen = callBackRewardFailedToOpen;
            _callBackRewardClose = callBackRewardClose;
            _callBackRewardCancel = callBackRewardCancel;
            _callBackRewardSuccess = callBackRewardSuccess;
            _rewardedAd.Show();
        }

        #endregion

        #region Common

        private AdRequest CreateRequest()
        {
            return new AdRequest.Builder().AddTestDevice("F3538E81D8FD7D1759F0B533784C8887").Build();
        }

        #endregion

        #region Banner

        private BannerView _bannerView;
        private bool _bannerShowing;

        private void RequestBanner()
        {
            _bannerView?.Destroy();
            _bannerView = new BannerView(bannerId, AdSize.SmartBanner, bannerPosition);
            _bannerView.OnAdOpening += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { OnAdsClicked?.Invoke(); });
            };
            _bannerView.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    MobileAdsEventExecutor.instance.StartCoroutine(IeInvokeDelay(15, RequestBanner));
                });
            };
            _bannerView.OnAdLoaded += (sender, args) => { MobileAdsEventExecutor.ExecuteInUpdate(() => { }); };
            _bannerView.LoadAd(CreateRequest());
            if (!_bannerShowing)
            {
                _bannerView.Hide();
            }
        }

        #endregion

        #region Interstitial

        private InterstitialAd _interstitialAd;

        private void RequestInterstitial()
        {
            _interstitialAd?.Destroy();
            _interstitialAd = new InterstitialAd(interstitialId);
            _interstitialAd.OnAdClosed += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(RequestInterstitial);
            };
            _interstitialAd.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    MobileAdsEventExecutor.instance.StartCoroutine(IeInvokeDelay(15, RequestInterstitial));
                });
            };
            _interstitialAd.OnAdLeavingApplication += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => OnAdsClicked?.Invoke());
            };
            _interstitialAd.LoadAd(CreateRequest());
        }

        #endregion

        #region Reward

        private RewardedAd _rewardedAd;
        private bool _gotReward;
        private Action _callBackRewardOpen;
        private Action _callBackRewardFailedToOpen;
        private Action _callBackRewardClose;
        private Action _callBackRewardCancel;
        private Action _callBackRewardSuccess;

        private void RequestReward()
        {
            _rewardedAd = new RewardedAd(rewardId);
            _rewardedAd.OnAdClosed += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    MobileAdsEventExecutor.instance.StartCoroutine(IeInvokeDelay(0.2f, () =>
                    {
                        if (_gotReward)
                        {
                            _callBackRewardSuccess?.Invoke();
                        }
                        else
                        {
                            _callBackRewardCancel?.Invoke();
                        }

                        _callBackRewardClose?.Invoke();
                        RequestReward();
                    }));
                });
            };
            _rewardedAd.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    MobileAdsEventExecutor.instance.StartCoroutine(IeInvokeDelay(15, RequestReward));
                });
            };
            _rewardedAd.OnAdFailedToShow += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { _callBackRewardFailedToOpen?.Invoke(); });
            };
            _rewardedAd.OnAdOpening += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    _gotReward = false;
                    _callBackRewardOpen?.Invoke();
                });
            };
            _rewardedAd.OnUserEarnedReward += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { _gotReward = true; });
            };
            _rewardedAd.OnAdLoaded += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { OnRewardLoaded?.Invoke(); });
            };
            _rewardedAd.LoadAd(CreateRequest());
        }

        private IEnumerator IeInvokeDelay(float time, Action callBack)
        {
            yield return new WaitForSeconds(time);
            callBack?.Invoke();
        }

        #endregion
    }
}