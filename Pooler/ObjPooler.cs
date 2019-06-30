using System;
using UnityEngine;

public class ObjPooler : MonoBehaviour, ObjPooler.IObjPooler
{
    private Action<ObjPooler> onAcitve;
    private Action<ObjPooler> onInactive;

    public virtual void Inactive()
    {
        gameObject.SetActive(false);
        onInactive?.Invoke(this);
    }

    public virtual void Active()
    {
        gameObject.SetActive(true);
        onAcitve?.Invoke(this);
    }

    void IObjPooler.SetEvent(Action<ObjPooler> onActive, Action<ObjPooler> onInactive)
    {
        this.onAcitve = onActive;
        this.onInactive = onInactive;
    }

    public interface IObjPooler
    {
        void SetEvent(Action<ObjPooler> onActive, Action<ObjPooler> onInActive);
    }
}
