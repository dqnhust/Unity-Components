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
        [SerializeField] private string bannerId;
        [SerializeField] private string interstitialId;
        [SerializeField] private string rewardId;
        [SerializeField] private AdPosition bannerPosition;

        #region Implement Interface

        public event Action OnRewardLoaded;
        public event Action OnAdsClicked;

        public void Init()
        {
            MobileAds.Initialize(initStatus => { Debug.Log("Mobile Ads Initialized" + initStatus); });
            RequestBanner();
            RequestInterstitial();
            RequestReward();
        }

        public void ShowInterstitial()
        {
            if (m_InterstitialAd.IsLoaded())
            {
                m_InterstitialAd.Show();
            }
        }

        public void ShowBanner()
        {
            m_BannerView?.Show();
            m_BannerShowing = true;
        }

        public void HideBanner()
        {
            m_BannerView?.Hide();
            m_BannerShowing = false;
        }

        public bool RewardVideoReady()
        {
            return m_RewardedAd.IsLoaded();
        }

        public void ShowReward(Action callBackRewardOpen, Action callBackRewardFailedToOpen, Action callBackRewardClose,
            Action callBackRewardCancel, Action callBackRewardSuccess)
        {
            if (!m_RewardedAd.IsLoaded())
            {
                callBackRewardFailedToOpen?.Invoke();
                return;
            }

            m_CallBackRewardOpen = callBackRewardOpen;
            m_CallBackRewardFailedToOpen = callBackRewardFailedToOpen;
            m_CallBackRewardClose = callBackRewardClose;
            m_CallBackRewardCancel = callBackRewardCancel;
            m_CallBackRewardSuccess = callBackRewardSuccess;
            m_RewardedAd.Show();
        }

        #endregion

        #region Common

        private AdRequest CreateRequest()
        {
            return new AdRequest.Builder().Build();
        }

        #endregion

        #region Banner

        private BannerView m_BannerView;
        private bool m_BannerShowing;

        private void RequestBanner()
        {
            m_BannerView?.Destroy();
            m_BannerView = new BannerView(bannerId, AdSize.Banner, bannerPosition);
            m_BannerView.OnAdOpening += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { OnAdsClicked?.Invoke(); });
            };
            m_BannerView.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(RequestBanner);
            };
            m_BannerView.OnAdLoaded += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    if (m_BannerShowing)
                    {
                        m_BannerView.Show();
                    }
                    else
                    {
                        m_BannerView.Hide();
                    }
                });
            };
            m_BannerView.LoadAd(CreateRequest());
        }

        #endregion

        #region Interstitial

        private InterstitialAd m_InterstitialAd;

        private void RequestInterstitial()
        {
            m_InterstitialAd?.Destroy();
            m_InterstitialAd = new InterstitialAd(interstitialId);
            m_InterstitialAd.OnAdClosed += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(RequestInterstitial);
            };
            m_InterstitialAd.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(RequestInterstitial);
            };
            m_InterstitialAd.OnAdLeavingApplication += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => OnAdsClicked?.Invoke());
            };
            m_InterstitialAd.LoadAd(CreateRequest());
        }

        #endregion

        #region Reward

        private RewardedAd m_RewardedAd;
        private bool m_GotReward;
        private Action m_CallBackRewardOpen;
        private Action m_CallBackRewardFailedToOpen;
        private Action m_CallBackRewardClose;
        private Action m_CallBackRewardCancel;
        private Action m_CallBackRewardSuccess;

        private void RequestReward()
        {
            m_RewardedAd = new RewardedAd(rewardId);
            m_RewardedAd.OnAdClosed += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    MobileAdsEventExecutor.instance.StartCoroutine(IeInvokeDelay(0.2f, () =>
                    {
                        if (m_GotReward)
                        {
                            m_CallBackRewardSuccess?.Invoke();
                        }
                        else
                        {
                            m_CallBackRewardCancel?.Invoke();
                        }

                        m_CallBackRewardClose?.Invoke();
                        RequestReward();
                    }));
                });
            };
            m_RewardedAd.OnAdFailedToLoad += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(RequestReward);
            };
            m_RewardedAd.OnAdFailedToShow += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { m_CallBackRewardFailedToOpen?.Invoke(); });
            };
            m_RewardedAd.OnAdOpening += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    m_GotReward = false;
                    m_CallBackRewardOpen?.Invoke();
                });
            };
            m_RewardedAd.OnUserEarnedReward += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { m_GotReward = true; });
            };
            m_RewardedAd.OnAdLoaded += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => { OnRewardLoaded?.Invoke(); });
            };
            m_RewardedAd.LoadAd(CreateRequest());
        }

        private IEnumerator IeInvokeDelay(float time, Action callBack)
        {
            yield return new WaitForSeconds(time);
            callBack?.Invoke();
        }

        #endregion
    }
}