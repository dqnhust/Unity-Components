using System;
using DataManager;
using Unity.Notifications.Android;
using UnityEngine;

namespace NotificationManager
{
    public interface INotificationManager
    {
        void Setup();
    }

    [CreateAssetMenu(menuName = "GameData/Notification/Create NotificationManager", fileName = "NotificationManager", order = 0)]
    public class NotificationManager : ScriptableObject, INotificationManager
    {
        [SerializeField] private CustomNotificationSetting defaultSetting;
        [SerializeField] private CustomNotificationSetting[] settings;

        private CustomNotificationSetting _cachedSetting;

        private CustomNotificationSetting GetSetting()
        {
            if (_cachedSetting == null)
            {
                foreach (var setting in settings)
                {
                    if (setting.targetLanguage == Application.systemLanguage)
                    {
                        _cachedSetting = setting;
                        break;
                    }
                }

                if (_cachedSetting == null)
                {
                    _cachedSetting = defaultSetting;
                }
            }

            return _cachedSetting;
        }

        private EventNumber<bool> CreatedChannel =>
            DataStorage.GetData("CreatedChannelNotification", new EventNumber<bool>(false));

        public void Setup()
        {
            if (!CreatedChannel.Value)
            {
                CreateNotificationChanel();
                CreatedChannel.Value = true;
            }

            CancelAllNotification();
            ScheduleNewNotification();
        }

        private void CancelAllNotification()
        {
            AndroidNotificationCenter.CancelNotification(GetSetting().pushSetting.id);
        }

        private void CreateNotificationChanel()
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = GetSetting().chanelSetting.chanelId,
                Name = GetSetting().chanelSetting.name,
                Importance = GetSetting().chanelSetting.importance,
                Description = GetSetting().chanelSetting.description,
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        private DateTime GetScheduleTime()
        {
            if (Debug.isDebugBuild)
            {
                return DateTime.Now.AddMinutes(1);
            }

            return DateTime.Now.AddDays(1);
        }

        private void ScheduleNewNotification()
        {
            var notification = new AndroidNotification();
            notification.Title = GetSetting().pushSetting.title;
            notification.Text = GetSetting().pushSetting.text;
            notification.FireTime = GetScheduleTime();
            notification.SmallIcon = "icon_0";
            notification.ShouldAutoCancel = true;
            notification.RepeatInterval = TimeSpan.FromDays(1);
            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, GetSetting().chanelSetting.chanelId,
                GetSetting().pushSetting.id);
        }
    }
}