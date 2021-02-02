using System;
using System.Collections.Generic;
using System.Linq;

namespace Analytics
{
    [Serializable]
    public class RetentionData
    {
        private readonly List<DateTime> _listTimePlay;

        public RetentionData(DateTime firstTime)
        {
            _listTimePlay = new List<DateTime> {firstTime};
        }

        public void OnUserPlayAndShowAds(DateTime currentTime)
        {
            _listTimePlay.Add(currentTime);
        }

        public bool GetReturnInDay(int dayCount)
        {
            var firstDayIndex = GetDayIndex(_listTimePlay.First());
            var days = _listTimePlay.Select(x => GetDayIndex(x) - firstDayIndex).Distinct().ToArray();
            return days.Contains(dayCount);
        }

        public bool GetRetentionInDay(int dayCount)
        {
            for (int i = 0; i <= dayCount; i++)
            {
                var haveReturnToPlay = GetReturnInDay(i);
                if (!haveReturnToPlay)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetLongestDayRetention()
        {
            for (int i = 0; i < 365; i++)
            {
                if (!GetRetentionInDay(i))
                {
                    return i - 1;
                }
            }

            return 0;
        }

        private int GetDayIndex(DateTime dateTime)
        {
            return (dateTime - DateTime.MinValue).Days;
        }
    }
}