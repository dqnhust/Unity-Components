using System;
using UnityEngine;

namespace DataManager
{
    public abstract class AbstractSavedDataObject<T> : ScriptableObject
    {
        public event Action<T> OnDataChanged;

        [SerializeField] protected string dataKey;

        public T Value
        {
            get => DataStorage.GetData(dataKey, default(T));
            set
            {
                DataStorage.SetData(dataKey, value);
                OnDataChanged?.Invoke(value);
            }
        }

        private T _value;
    }
}