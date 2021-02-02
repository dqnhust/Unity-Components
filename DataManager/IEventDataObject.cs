using System;

namespace DataManager
{
    public interface IEventDataObject
    {
        string GetValueString();
        event Action OnDataObjectChanged;
    }
}