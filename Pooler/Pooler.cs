using System.Collections.Generic;
using UnityEngine;

public abstract class Pooler<T> : MonoBehaviour where T : ObjPooler
{
    [SerializeField] protected T template;
    protected List<T> listActive = new List<T>();
    protected List<T> listInactive = new List<T>();
    protected List<T> listWorking = new List<T>();

    public T GetObj()
    {
        T instance;
        if (listInactive.Count > 0)
        {
            instance = (T)listInactive[0];
            listInactive.RemoveAt(0);
        }
        else
        {
            var g = Instantiate(template.gameObject, template.transform.parent) as GameObject;
            instance = g.GetComponent<T>();
            instance.SetEvents(() => { OnObjActive(instance); }, () => { OnObjInActive(instance); });
        }
        listWorking.Add(instance);
        return instance;
    }

    public List<T> GetListActive()
    {
        return listActive;
    }

    public List<T> GetListInactive()
    {
        return listInactive;
    }

    public void Cache(int instanceCount)
    {
        for (int i = 0; i < instanceCount; i++)
        {
            var g = Instantiate(template.gameObject, template.transform.parent) as GameObject;
            T instance = g.GetComponent<T>();
            instance.SetEvents(() => { OnObjActive(instance); }, () => { OnObjInActive(instance); });
            listInactive.Add(instance);
        }
    }

    public void InactiveAll()
    {
        foreach (var item in listActive.ToArray())
        {
            item.Inactive();
        }
        foreach (var item in listWorking.ToArray())
        {
            item.Inactive();
        }
    }

    public void ActiveAll()
    {
        foreach (var item in listInactive.ToArray())
        {
            item.Active();
        }
        foreach (var item in listWorking.ToArray())
        {
            item.Active();
        }
    }

    #region Active For ObjPooler
    private void OnObjInActive(T instance)
    {
        listInactive.Add(instance);
        listActive.Remove(instance);
        listWorking.Remove(instance);
    }

    private void OnObjActive(T instance)
    {
        listActive.Add(instance);
        listInactive.Remove(instance);
        listWorking.Remove(instance);
    }
    #endregion
}
