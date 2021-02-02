using System;
using UnityEngine;

namespace DataManager
{
    public abstract class AbstractSavedDataObject<T> : ScriptableObject, IEventDataObject
    {
        public virtual string GetValueString()
        {
            return Value.ToString();
        }

        public event Action OnDataObjectChanged;

        public event Action<T> OnDataChanged;

        [SerializeField] protected string dataKey;
        [SerializeField] protected T defaultValue;

        public T Value
        {
            get => DataStorage.GetData(dataKey, defaultValue);
            set
            {
                DataStorage.SetData(dataKey, value);
                OnDataChanged?.Invoke(value);
                OnDataObjectChanged?.Invoke();
            }
        }
    }
}