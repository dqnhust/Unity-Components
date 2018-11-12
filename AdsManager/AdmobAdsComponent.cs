using System;
using GoogleMobileAds.Api;

namespace MobileLandTemplate
{
    public class AdmobAdsComponent : IAds, IBanner
    {

        public AdmobAdsComponent(string appId)
        {
            MobileAds.Initialize(appId);
        }
        #region Banner
        private BannerView bannerView;
        public AdmobAdsComponent InitBanner(string bannerId, AdPosition bannerPosition)
        {
            bannerView = new BannerView(bannerId, AdSize.Banner, bannerPosition);
            var adRequest = new AdRequest.Builder().Build();
            bannerView.OnAdClosed += (sender, e) =>
            {
                bannerView.Destroy();
                bannerView.LoadAd(adRequest);
            };
            bannerView.LoadAd(adRequest);
            return this;
        }

        public void ShowBanner()
        {
            if (bannerView != null)
                bannerView.Show();
        }

        public void HideBanner()
        {
            if (bannerView != null)
                bannerView.Hide();
        }
        #endregion
        #region Ads
        private InterstitialAd interstitialAd;
        public AdmobAdsComponent InitAds(string interstitialId)
        {
            AdRequest adRequest = new AdRequest.Builder().Build();
            interstitialAd = new InterstitialAd(interstitialId);
            interstitialAd.OnAdClosed += (sender, e) =>
            {
                interstitialAd.Destroy();
                interstitialAd.LoadAd(adRequest);
            };
            interstitialAd.LoadAd(adRequest);
            return this;
        }

        public bool AdsReady
        {
            get
            {
                return interstitialAd != null && interstitialAd.IsLoaded();
            }
        }

        public void ShowAds()
        {
            if (interstitialAd == null) return;
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
            }
        }
        #endregion
        #region Reward
        private bool rewarded = false;
        private string rewardId;
        private System.Action rewardCallBackDone;
        private System.Action rewardCallBackFailed;

        private RewardBasedVideoAd rewardBasedVideo;
        public AdmobAdsComponent InitRewardAds(string rewardId)
        {
            this.rewardId = rewardId;
            this.rewardBasedVideo = RewardBasedVideoAd.Instance;
            this.RequestRewardBasedVideo();
            this.rewardBasedVideo.OnAdClosed += RewardBasedVideo_OnAdClosed;
            this.rewardBasedVideo.OnAdRewarded += RewardBasedVideo_OnAdRewarded;
            return this;
        }

        void RewardBasedVideo_OnAdClosed(object sender, EventArgs e)
        {
            if (rewarded)
            {
                rewardCallBackDone.Invoke();
            }
            else
            {
                rewardCallBackFailed.Invoke();
            }
            this.RequestRewardBasedVideo();
        }

        void RewardBasedVideo_OnAdRewarded(object sender, Reward e)
        {
            this.rewarded = true;
        }

        public void ShowReward(Action callBackReward, Action callBackFail)
        {
            if (!RewardReady)
            {
                callBackFail.Invoke();
                return;
            }
            this.rewarded = false;
            this.rewardCallBackDone = callBackReward;
            this.rewardCallBackFailed = callBackFail;
            rewardBasedVideo.Show();
        }

        private void RequestRewardBasedVideo()
        {
            AdRequest request = new AdRequest.Builder().Build();
            this.rewardBasedVideo.LoadAd(request, rewardId);
        }


        public bool RewardReady
        {
            get
            {
                return rewardBasedVideo != null && rewardBasedVideo.IsLoaded();
            }
        }
        #endregion
    }
}