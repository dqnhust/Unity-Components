#pragma warning disable 0649
using System;
using System.Globalization;
using System.Linq;
using DataManager;
using Firebase.Analytics;
using UnityEngine;

namespace Analytics
{
    [CreateAssetMenu(menuName = "GameData/Create AnalyticController", fileName = "AnalyticController", order = 0)]
    public class AnalyticController : ScriptableObject
    {
        private const string ParamLocalTime = "local_time";
        private RetentionData RetentionData => DataStorage.GetData("RetentionData", new RetentionData(DateTime.Now));


        public void Setup()
        {
            Debug.Log("AnalyticController Initializing!");
            Initialized = true;
            LogEvent(FirebaseAnalytics.EventAppOpen,
                (ParamLocalTime, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                ("app_version_name", Application.version));
            RetentionData.OnUserPlayAndShowAds(DateTime.Now);
            LogEvent($"reach_retention_d{RetentionData.GetLongestDayRetention()}");
            Debug.Log("AnalyticController Initialized!");
        }

        private bool Initialized { get; set; }

        #region Basic Method

        private bool EnableLog
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    return false;
                }
#endif
                if (Debug.isDebugBuild)
                {
                    return false;
                }

                if (!Initialized)
                {
                    return false;
                }

                return true;
            }
        }

        public void LogEvent(string eventName)
        {
            DebugLogEvent(eventName);
            if (!EnableLog)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(eventName);
        }

        public void LogEvent(string eventName, params (string paramName, string paramValue)[] parameters)
        {
            DebugLogEvent(eventName, parameters);
            if (!EnableLog)
            {
                return;
            }

            var data = parameters.Select(x => new Parameter(x.paramName, x.paramValue)).ToArray();
            FirebaseAnalytics.LogEvent(eventName, data);
        }

        public void LogEvent(string eventName, params (string paramName, int paramValue)[] parameters)
        {
            DebugLogEvent(eventName, parameters.Select(x => (x.paramName, x.paramValue.ToString())).ToArray());
            if (!EnableLog)
            {
                return;
            }
            var data = parameters.Select(x => new Parameter(x.paramName, x.paramValue)).ToArray();
            FirebaseAnalytics.LogEvent(eventName, data);
        }

        public void LogEvent(string eventName, string paramName, string paramValue) =>
            LogEvent(eventName, (paramName, paramValue));
        public void LogEvent(string eventName, string paramName, int paramValue) =>
            LogEvent(eventName, (paramName, paramValue));

        private void DebugLogEvent(string eventName, params (string paramName, string paramValue)[] parameters)
        {
            var paramString = "";
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    paramString += (item.paramName, item.paramValue) + "\n";
                }
            }

            if (parameters != null && parameters.Length > 0)
            {
                Debug.Log($"[Analytics] EventName:{eventName} => {paramString}");
            }
            else
            {
                Debug.Log($"[Analytics] EventName:{eventName}");
            }
        }

        #endregion

    }
}