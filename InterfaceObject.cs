#pragma warning disable 0649
using UnityEngine;

[System.Serializable]
public class InterfaceObject<T>
{
    [SerializeField] private Object objTarget;

    public T Value
    {
        get
        {
            if (objTarget is T o)
            {
                return o;
            }

            if (objTarget is GameObject g)
            {
                var c = g.GetComponent<T>();
                if (c != null)
                {
                    return c;
                }
            }
            Debug.LogError($"Can't cast {objTarget.GetType()} to {typeof(T)}", objTarget);
            return default(T);
        }
    }

    //
    // public static implicit operator T(InterfaceObject<T> obj)
    // {
    //     return obj.Value;
    // }
}