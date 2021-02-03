namespace Analytics
{
    public static class AnalyticControllerExtension
    {
        public static void LogShare(this AnalyticController controller)
        {
            controller.LogEvent("share");
        }

        public static void LogOpenRateLink(this AnalyticController controller)
        {
            controller.LogEvent("open_rate_link");
        }

        public static void LogPassedLevel(this AnalyticController controller, int levelIndex)
        {
            controller.LogEvent($"passed_level_{levelIndex}");
        }

        public static void LogSkipLevel(this AnalyticController controller, int levelIndex)
        {
            controller.LogEvent($"skipped_level_{levelIndex}");
        }
        
        public static void LogFailedLevel(this AnalyticController controller, int levelIndex)
        {
            controller.LogEvent($"failed_level_{levelIndex}");
        }

        public static void SetPropertyPassedLevel(this AnalyticController controller, int levelCount)
        {
            controller.SetUserProperty("passed_level_count", levelCount.ToString());
        }

        public static void SetPropertyFailedLevel(this AnalyticController controller, int levelCount)
        {
            controller.SetUserProperty("failed_level_count", levelCount.ToString());
        }
        
        public static void SetPropertySkippedLevel(this AnalyticController controller, int count)
        {
            controller.SetUserProperty("skipped_level_count", count.ToString());
        }
        
        public static void SetPropertyWatchedInterstitialCount(this AnalyticController controller, int count)
        {
            controller.SetUserProperty("interstitial_count", count.ToString());
        }

    }
}