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
    }
}