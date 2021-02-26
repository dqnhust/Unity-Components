using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace NotificationManager
{
    [CreateAssetMenu(menuName = "GameData/Notification/Create NotificationSetting", fileName = "NotificationSetting", order = 0)]
    public class CustomNotificationSetting : ScriptableObject
    {
        public SystemLanguage targetLanguage;

        [Serializable]
        public class ChanelSetting
        {
            public string chanelId;
            public string name;
            public Importance importance;
            [Multiline] public string description;
        }

        [Serializable]
        public class PushSetting
        {
            public string title;
            [Multiline] public string text;
            public int id;
        }


        public ChanelSetting chanelSetting;
        public PushSetting pushSetting;
    }
}