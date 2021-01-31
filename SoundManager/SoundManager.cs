using UnityEngine;

namespace SoundManager
{
    public class SoundManager : AbstractSoundManager
    {
        [SerializeField] private SoundManagerEvent soundManagerEvent;

        protected override void Init()
        {
            base.Init();
            soundManagerEvent.OnCallPlayClip += SoundManagerEventOnOnCallPlayClip;
            soundManagerEvent.OnCallPlayLoop += SoundManagerEventOnOnCallPlayLoop;
            soundManagerEvent.OnCallStopLoop += SoundManagerEventOnOnCallStopLoop;
        }

        private void OnDestroy()
        {
            soundManagerEvent.OnCallPlayClip -= SoundManagerEventOnOnCallPlayClip;
            soundManagerEvent.OnCallPlayLoop -= SoundManagerEventOnOnCallPlayLoop;
            soundManagerEvent.OnCallStopLoop -= SoundManagerEventOnOnCallStopLoop;
        }

        private void SoundManagerEventOnOnCallStopLoop(AudioClip obj)
        {
            StopLoop(obj);
        }

        private void SoundManagerEventOnOnCallPlayLoop(AudioClip obj)
        {
            PlayLoop(obj);
        }

        private void SoundManagerEventOnOnCallPlayClip(AudioClip obj)
        {
            PlayEffect(obj);
        }
    }
}