using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractSoundManager : MonoBehaviour
{
    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("MusicVolume", 1);
        }
        set
        {
            if (value.Equals(MusicVolume)) return;
            PlayerPrefs.SetFloat("MusicVolume", value);
            InvokeEvent(OnMusicVolumeChange);
        }
    }

    public float SoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("SoundVolume", 1);
        }
        set
        {
            if (value.Equals(SoundVolume)) return;
            PlayerPrefs.SetFloat("SoundVolume", value);
            InvokeEvent(OnSoundVolumeChange);
        }
    }

    public event System.Action OnMusicVolumeChange;
    public event System.Action OnSoundVolumeChange;

    protected AudioSource effectSource;
    protected Transform loopStore;

    protected void InvokeEvent(System.Action callBack)
    {
        if (callBack != null)
        {
            callBack.Invoke();
        }
    }

    protected bool inited = false;

    protected virtual void Awake()
    {
        Init();
    }

    protected void Init()
    {
        if (inited) return;
        if (effectSource == null)
        {
            effectSource = gameObject.AddComponent<AudioSource>();
        }

        if (loopStore == null)
        {
            loopStore = transform;
        }
        inited = true;
    }

    protected void PlayEffect(AudioClip clip)
    {
        if (!inited)
        {
            Init();
        }
        effectSource.PlayOneShot(clip);
    }

    private Dictionary<AudioClip, AudioSource> loopDict = new Dictionary<AudioClip, AudioSource>();

    protected AudioSource PlayLoop(AudioClip clip)
    {
        AudioSource source;
        bool exist = loopDict.TryGetValue(clip, out source);
        if (exist)
        {
            source.Play();
        }
        else
        {
            source = loopStore.gameObject.AddComponent<AudioSource>();
            source.loop = true;
            source.playOnAwake = false;
            source.clip = clip;
            source.Play();
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
        AudioSource source;
        loopDict.TryGetValue(clip, out source);
        return source;
    }
}