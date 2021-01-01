#pragma warning disable 0649
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class AbstractInterfaceObject
{
    public abstract Type TargetType { get; }
}

[System.Serializable]
public class InterfaceObject<T> : AbstractInterfaceObject
{
    [SerializeField] private Object objTarget;

    public T Value
    {
        get
        {
            if (objTarget == null)
            {
                Debug.LogError($"{typeof(T).Name} cannot be null!");
                return default;
            }

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
            return default;
        }
    }

    //
    // public static implicit operator T(InterfaceObject<T> obj)
    // {
    //     return obj.Value;
    // }
    public override Type TargetType => typeof(T);
}