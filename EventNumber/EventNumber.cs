#pragma warning disable 0649
using System;

[Serializable]
public class EventNumber<T>
{
    private T value;

    public T Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            OnValueChanged?.Invoke();
        }
    }


    [field: NonSerializedAttribute]
    public event System.Action OnValueChanged;

    public EventNumber(T value)
    {
        this.value = value;
    }
}