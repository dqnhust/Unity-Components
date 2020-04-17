#pragma warning disable 0649
using System.Collections;
using UnityEngine;

public class AnimationClipController : MonoBehaviour
{
    [SerializeField] private AnimationClip clip;
    [SerializeField] private GameObject target;


    public AnimationClip Clip => clip;
    public GameObject Target => target;

    public float ClipTime
    {
        get => _clipTime;
        set
        {
            _clipTime = Mathf.Clamp(value, 0, clip.length);
            SampleAnimation(_clipTime);
        }
    }

    private float _clipTime = 0;

    public float ClipTimePercent
    {
        get => ClipTime / clip.length;
        set => ClipTime = value * clip.length;
    }

    public float ClipLength => clip.length;

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
            SampleAnimation(t);
            yield return null;
        }
        SampleAnimation(toTime);
    }

    private void SampleAnimation(float time)
    {
        clip.SampleAnimation(target, time);
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
}
