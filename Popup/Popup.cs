using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup:MonoBehaviour
{
    [SerializeField] private Animator anim;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        anim.Play("Idle", 0, 0);
        anim.Update(Time.unscaledDeltaTime);
        OnOpen();
    }

    public virtual void Close()
    {
        anim.Play("Idle", 0, 0);
        anim.Update(Time.unscaledDeltaTime);
        OnClose();
        gameObject.SetActive(false);
    }

    public virtual void OpenWithEffect()
    {
        gameObject.SetActive(true);
        anim.Play("FadeIn", 0, 0);
        anim.Update(Time.unscaledDeltaTime);
    }

    public virtual void CloseWithEffect()
    {
        anim.Play("FadeOut", 0, 0);
        anim.Update(Time.unscaledDeltaTime);
    }

    protected virtual void OnOpen()
    {
        
    }

    protected virtual void OnClose()
    {

    }

    public virtual void Init()
    {
        
    }
}

