#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipController : MonoBehaviour
{
    [SerializeField] private AnimationClip clip;
    [SerializeField] private GameObject target;
    [Space]
    [SerializeField] private Event[] events;

    public AnimationClip Clip => clip;
    public GameObject Target => target;

    private bool updatedView = false;

    public float ClipTime
    {
        get => _clipTime;
        set
        {
            var oldTime = _clipTime;
            if (updatedView && value == oldTime)
            {
                return;
            }
            _clipTime = Mathf.Clamp(value, 0, clip.length);
            SampleAnimation(oldTime, _clipTime);
            updatedView = true;
        }
    }
    private float _clipTime = 0;

    private void OnEnable()
    {
        ClipTime = 0;
    }


    public float ClipTimePercent
    {
        get => ClipTime / clip.length;
        set => ClipTime = value * clip.length;
    }

    public float ClipLength => clip.length;
    public bool AssignedOk => clip != null && target != null;

    public Coroutine Play(float speed = 1) => Play(0, ClipLength, speed);
    public Coroutine Play(float fromTime, float toTime, float speed)
    {
        fromTime = Mathf.Clamp(fromTime, 0, ClipLength);
        toTime = Mathf.Clamp(toTime, 0, ClipLength);
        if (Mathf.Sign(toTime - fromTime) * speed <= 0)
        {
            Debug.LogError("Something went wrong!");
            return null;
        }
        return StartCoroutine(IEPlay(fromTime, toTime, speed));
    }

    private IEnumerator IEPlay(float fromTime, float toTime, float speed = 1)
    {
        for (float t = fromTime; t * speed <= toTime * speed; t += Time.deltaTime * speed)
        {
            ClipTime = t;
            yield return null;
        }
        ClipTime = toTime;
    }

    private List<Event> methodWillInvoke = new List<Event>();
    private void SampleAnimation(float oldTime, float time)
    {
        clip.SampleAnimation(target, time);
        methodWillInvoke.Clear();
        foreach (var item in events)
        {
            if ((oldTime - item.time) * (item.time - time) >= 0)
            {
                methodWillInvoke.Add(item);
                item.callBack.Invoke();
            }
        }
        methodWillInvoke.Sort((e1, e2) =>
        {
            return Mathf.Abs(oldTime - e1.time).CompareTo(Mathf.Abs(oldTime - e2.time));
        });
        foreach (var item in methodWillInvoke)
        {
            item.callBack.Invoke();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Open Animation Window")]
    private void OpenAnimationWindow()
    {

        Animation animation;
        animation = GetComponent<Animation>();
        if (animation == null)
            animation = gameObject.AddComponent<Animation>();
        animation.clip = clip;
        animation.AddClip(clip, clip.name);
        UnityEditor.EditorApplication.ExecuteMenuItem("Window/Animation/Animation");
    }

    [ContextMenu("Cleanup Trash")]
    private void CleanupTrash()
    {

        Animation animation;
        animation = GetComponent<Animation>();
        if (animation != null)
            DestroyImmediate(animation);
    }
#endif
    [System.Serializable]
    public class Event
    {
        public float time;
        public UnityEngine.Events.UnityEvent callBack;
    }
}
