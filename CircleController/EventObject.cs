using System;
using CircleController;
using UnityEngine;

namespace DqnAsset.CircleController
{
    [CreateAssetMenu(menuName = "GameData/Create EventObject", fileName = "EventObject", order = 0)]
    public class EventObject : ScriptableObject, ICircleController
    {
        public Vector2 Direction { get; set; } = Vector2.zero;
        public event Action<Vector2> OnChanged;
        public event Action OnReleased;

        public void InvokeOnChanged(Vector2 direction)
        {
            OnChanged?.Invoke(direction);
        }

        public void InvokeOnReleased()
        {
            OnReleased?.Invoke();
        }
    }
}