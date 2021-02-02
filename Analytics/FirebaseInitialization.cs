using System;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Analytics
{
    public class FirebaseInitialization : MonoBehaviour
    {
        [SerializeField] private UnityEvent onInitialized;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            Debug.Log("FirebaseInitialization Start Check!");
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    onInitialized?.Invoke();
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }
    }
}