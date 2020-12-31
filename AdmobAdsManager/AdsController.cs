#pragma warning disable 0649
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
        [SerializeField] private EventSavedBoolObject boughIap;

        [SerializeField] private GameManagerEvent gameManagerEvent;
        // [SerializeField] private CurrentGameModeStore currentGameModeStore;
        // [SerializeField] private RateAndReviewController rateAndReviewController;

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
            boughIap.OnValueChanged += OnBoughIAPValueChanged;
            gameManagerEvent.EventShowGameOverUI += OnGameOver;
            // gameManagerEvent.EventGameGenerated += OnGameGenerated;
            // gameManagerEvent.EventGamePaused += OnGamePause;
            if (!boughIap.GetValue())
            {
                ShowBanner();
            }
            else
            {
                HideBanner();
            }

            gameManagerEvent.EventShowReward += OnHaveCallShowReward;
            // TryShowInterstitial();
        }

        private void OnDisable()
        {
            gameManagerEvent.EventShowGameOverUI -= OnGameOver;
            // gameManagerEvent.EventGamePaused -= OnGamePause;
            // gameManagerEvent.EventGameGenerated -= OnGameGenerated;
            boughIap.OnValueChanged -= OnBoughIAPValueChanged;
            gameManagerEvent.EventShowReward -= OnHaveCallShowReward;
        }

        // private void TryShowInterstitial()
        // {
        //     
        //     var gameMode = obj.GetComponent<IGameMode>();
        //     if (gameMode is IScoreGameMode scoreGameMode)
        //     {
        //         if (scoreGameMode.ScoreStore.BestScore > 500)
        //         {
        //             StartCoroutine(IeShowDelay(2f));
        //         }
        //     }
        // }

        // private bool needShowFirst = true;
        //
        // private void OnGameGenerated(GameObject obj)
        // {
        //     if (!needShowFirst)
        //     {
        //         return;
        //     }
        //
        //     needShowFirst = false;
        //     var gameMode = obj.GetComponent<IGameMode>();
        //     if (gameMode is IScoreGameMode scoreGameMode)
        //     {
        //         if (scoreGameMode.ScoreStore.BestScore > 500)
        //         {
        //             StartCoroutine(IeShowDelay(2f));
        //         }
        //     }
        // }

        private void OnHaveCallShowReward()
        {
            // gameManagerEvent.OnEventFailedToShowReward();
            // return;
            adsManager.ShowRewarded(gameManagerEvent.OnEventRewardOpen, gameManagerEvent.OnEventGotRewarded,
                gameManagerEvent.OnEventFailedToShowReward, gameManagerEvent.OnEventCloseReward);
        }

        private void OnBoughIAPValueChanged()
        {
            if (boughIap.GetValue())
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
            if (boughIap.GetValue())
            {
                return;
            }

            StartCoroutine(IeShowDelay(0.5f));
            // adsManager.ShowInterstitial();
        }


        private IEnumerator IeShowDelay(float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            // if (rateAndReviewController.Showing)
            // {
            //     yield break;
            // }

            ShowInterstitial();
        }

        private float _lastTimePause;

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _lastTimePause = Time.realtimeSinceStartup;
            }
            else
            {
                var deltaTime = Time.realtimeSinceStartup - _lastTimePause;
                if (deltaTime > 20)
                {
                    ShowInterstitial();
                }
            }
        }

        private void ShowInterstitial()
        {
            CallShowInterstitialCount++;
            if (boughIap.GetValue())
            {
                return;
            }

            adsManager.ShowInterstitial();
        }

        private void ShowBanner()
        {
            if (boughIap.GetValue())
            {
                return;
            }

            adsManager.ShowBanner();
        }

        private void HideBanner()
        {
            adsManager.HideBanner();
        }
    }
}