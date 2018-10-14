using UnityEngine;

public abstract class ObjPooler : MonoBehaviour
{
    private System.Action callBackActive;
    private System.Action callBackInActive;

    /// <summary>
    /// Call By Pooler System
    /// </summary>
    /// <param name="callBackActive">Call back when active.</param>
    /// <param name="callBackInActive">Call back when inactive.</param>
    public void SetEvents(System.Action callBackActive, System.Action callBackInActive)
    {
        this.callBackActive = callBackActive;
        this.callBackInActive = callBackInActive;
    }

    public virtual void Inactive()
    {
        if (callBackInActive != null)
        {
            callBackInActive.Invoke();
        }
        gameObject.SetActive(false);
    }

    public virtual void Active()
    {
        if (callBackActive != null)
        {
            callBackActive.Invoke();
        }
        gameObject.SetActive(true);
    }
}