#pragma warning disable 0649
using System;
using System.Collections;
using GameEvent;
using GameMode;
using InAppReview;
using UnityEngine;

namespace Ads
{
    public class AdsController : MonoBehaviour
    {
        [SerializeField] private AdsManager adsManager;
        [SerializeField] private EventSavedBoolObject boughIAP;
        [SerializeField] private GameManagerEvent gameManagerEvent;
        [SerializeField] private InAppReviewController inAppReviewController;

        private int CallShowInterstitialCount
        {
            get => PlayerPrefs.GetInt("CallShowInterstitialCount", 0);
            set => PlayerPrefs.SetInt("CallShowInterstitialCount", value);
        }

        private void Awake()
        {
            // if (!boughIAP.GetValue())
            // {
            // }
            adsManager.Init(true, true, true);
        }

        private void OnEnable()
        {
            boughIAP.OnValueChanged += OnBoughIAPValueChanged;
            gameManagerEvent.EventShowGameOverUI += OnGameOver;
            // gameManagerEvent.EventGamePaused += OnGamePause;
            if (!boughIAP.GetValue())
            {
                adsManager.ShowBanner();
            }
            else
            {
                adsManager.HideBanner();
            }

            gameManagerEvent.EventShowReward += OnHaveCallShowReward;
        }

        private void OnDisable()
        {
            gameManagerEvent.EventShowGameOverUI -= OnGameOver;
            // gameManagerEvent.EventGamePaused -= OnGamePause;
            boughIAP.OnValueChanged -= OnBoughIAPValueChanged;
            gameManagerEvent.EventShowReward -= OnHaveCallShowReward;
        }

        private void OnHaveCallShowReward()
        {
            adsManager.ShowRewarded(gameManagerEvent.OnEventGotRewarded, gameManagerEvent.OnEventCloseReward);
        }

        private void OnBoughIAPValueChanged()
        {
            if (boughIAP.GetValue())
            {
                adsManager.HideBanner();
            }
        }

        // private void OnGamePause()
        // {
        //     if (boughIAP.GetValue()) return;
        //     if (CallShowInterstitialCount > 1)
        //     {
        //         adsManager.ShowInterstitial();
        //     }
        // }

        private void OnGameOver(IGameMode obj)
        {
            CallShowInterstitialCount++;
            if (boughIAP.GetValue())
            {
                return;
            }

            StartCoroutine(IeShowDelay(1f));
            // adsManager.ShowInterstitial();
        }


        private IEnumerator IeShowDelay(float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (inAppReviewController.Showing)
            {
                yield break;
            }

            adsManager.ShowInterstitial();
        }

        private float lastTimePause;

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                lastTimePause = Time.realtimeSinceStartup;
            }
            else
            {
                var deltaTime = Time.realtimeSinceStartup - lastTimePause;
                if (deltaTime > 20)
                {
                    adsManager.ShowInterstitial();
                }
            }
        }
    }
}