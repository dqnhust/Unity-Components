using System;
using UnityEngine;

namespace ToastView
{
    [CreateAssetMenu(menuName = "GameData/Create ToastEvent", fileName = "ToastEvent", order = 0)]
    public class ToastEvent : ScriptableObject
    {
        public event Action<string> EventShowToast;

        public void InvokeEventShowToast(string message)
        {
            EventShowToast?.Invoke(message);
        }
    }
}