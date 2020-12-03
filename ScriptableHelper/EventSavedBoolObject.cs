#pragma warning disable 0649
using UnityEngine;

namespace GameEvent
{
    [CreateAssetMenu(menuName = "GameData/EventSavedBoolObject", fileName = "EventSavedBoolObject", order = 0)]
    public class EventSavedBoolObject : AbstractEventObject<bool>
    {
        [SerializeField] private bool defaultValue;

        private bool SavedValue
        {
            get => PlayerPrefs.GetInt(name + "Scriptable", defaultValue ? 1 : 0) == 1;
            set => PlayerPrefs.SetInt(name + "Scriptable", value ? 1 : 0);
        }

        protected override bool Value
        {
            get => SavedValue;
            set => SavedValue = value;
        }
    }
}