using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    public virtual void Close()
    {
        OnClose();
        gameObject.SetActive(false);
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