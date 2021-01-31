using System;
using UnityEngine;

namespace SoundManager
{
    [CreateAssetMenu(menuName = "GameData/Create SoundManagerEvent", fileName = "SoundManagerEvent", order = 0)]
    public class SoundManagerEvent : ScriptableObject
    {
        public event Action<AudioClip> OnCallPlayClip;

        public void InvokeOnCallPlayClip(AudioClip clip)
        {
            OnCallPlayClip?.Invoke(clip);
        }
        
        public event Action<AudioClip> OnCallPlayLoop;

        public void InvokeOnCallPlayLoop(AudioClip clip)
        {
            OnCallPlayLoop?.Invoke(clip);
        }
        
        public event Action<AudioClip> OnCallStopLoop;

        public void InvokeOnCallStopLoop(AudioClip clip)
        {
            OnCallStopLoop?.Invoke(clip);
        }
    }
}