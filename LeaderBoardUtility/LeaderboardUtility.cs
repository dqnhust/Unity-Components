using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames.BasicApi;
using GooglePlayGames;
#endif
namespace VnzITComponent
{
    public static class LeaderboardUtility
    {
        public static void Authenticate(System.Action<bool> callBackAuthenticate)
        {
#if UNITY_IOS
        IosAuthenticate(callBackAuthenticate);
#endif
#if UNITY_ANDROID
            AndroidAuthenticate(callBackAuthenticate);
#endif
        }

        public static void ReportScore(long score, string leaderboardId, System.Action<bool> callBackReportScore)
        {
            Social.ReportScore(score, leaderboardId, callBackReportScore);
        }

        public static void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
        }
#if UNITY_IOS

    private static void IosAuthenticate(System.Action<bool> callBackAuthenticate)
    {
        Social.localUser.Authenticate(callBackAuthenticate);
    }
#endif
#if UNITY_ANDROID
        private static void AndroidAuthenticate(System.Action<bool> callBackAuthenticate)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(callBackAuthenticate);
        }
#endif
    }
}