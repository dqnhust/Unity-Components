using UnityEngine;
using System.Collections.Generic;

namespace SoundManager
{
    public abstract class AbstractSoundManager : MonoBehaviour
    {
        private AudioSource _effectSource;
        private Transform _loopStore;

        private bool _initialized;

        protected float SoundVolume
        {
            get => _effectSource.volume;
            set => _effectSource.volume = value;
        }

        private float _musicVolume;

        protected float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                foreach (var data in LoopDict)
                {
                    data.Value.volume = value;
                }
            }
        }

        protected virtual void Awake()
        {
            Init();
        }

        protected void Init()
        {
            if (_initialized) return;
            if (_effectSource == null)
            {
                _effectSource = gameObject.AddComponent<AudioSource>();
            }

            if (_loopStore == null)
            {
                _loopStore = transform;
            }

            _initialized = true;
        }

        protected void PlayEffect(AudioClip clip)
        {
            if (!_initialized)
            {
                Init();
            }

            _effectSource.PlayOneShot(clip);
        }

        private Dictionary<AudioClip, AudioSource> LoopDict { get; } = new Dictionary<AudioClip, AudioSource>();

        protected AudioSource PlayLoop(AudioClip clip)
        {
            bool exist = LoopDict.TryGetValue(clip, out var source);
            if (exist)
            {
                source.Play();
            }
            else
            {
                source = _loopStore.gameObject.AddComponent<AudioSource>();
                source.loop = true;
                source.volume = MusicVolume;
                source.playOnAwake = false;
                source.clip = clip;
                source.Play();
                LoopDict.Add(clip, source);
            }

            return source;
        }

        protected void StopLoop(AudioSource source)
        {
            source.Stop();
        }

        protected void StopLoop(AudioClip clip)
        {
            AudioSource source = GetLoopingSource(clip);
            if (source != null)
            {
                source.Stop();
            }
        }

        protected AudioSource GetLoopingSource(AudioClip clip)
        {
            LoopDict.TryGetValue(clip, out var source);
            return source;
        }
    }
}