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

            Debug.LogError($"Can't cast {objTarget.GetType()} to {typeof(T)}");
            return default(T);
        }
    }
}