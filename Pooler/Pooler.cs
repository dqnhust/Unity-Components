﻿using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    private Dictionary<int, PoolerStore> dict = new Dictionary<int, Pooler.PoolerStore>();

    public T GetObj<T>(T template) where T : ObjPooler
    {
        if (template == null)
            return null;
        int id = template.GetInstanceID();
        var poolerStore = GetPooler(id);
        return (T)poolerStore.GetObj(template);
    }

    public T GetObj<T>(T template, Transform parent) where T : ObjPooler
    {
        if (template == null)
            return null;
        int id = template.GetInstanceID();
        var poolerStore = GetPooler(id);
        return (T)poolerStore.GetObj(template, parent);
    }

    public void InActiveAll()
    {
        foreach (var item in dict)
        {
            item.Value.InactiveAll();
        }
    }

    public PoolerStore GetPooler<T>(T template) where T : ObjPooler
    {
        var id = template.GetInstanceID();
        return GetPooler(id);
    }

    private PoolerStore GetPooler(int id)
    {
        PoolerStore pooler;
        if (!dict.TryGetValue(id, out pooler))
        {
            pooler = new PoolerStore();
            dict.Add(id, pooler);
        }
        return pooler;
    }

    public class PoolerStore
    {
        private HashSet<ObjPooler> listInactive = new HashSet<ObjPooler>();
        private HashSet<ObjPooler> listWorking = new HashSet<ObjPooler>();
        private HashSet<ObjPooler> listActive = new HashSet<ObjPooler>();

        public ObjPooler GetObj(ObjPooler template, Transform parent)
        {
            var obj = GetObj(template);
            obj.transform.SetParent(parent);
            return obj;
        }

        public ObjPooler GetObj(ObjPooler template)
        {
            if (template == null)
            {
                return null;
            }
            ObjPooler item = null;
            foreach (var inactiveItem in listInactive)
            {
                item = inactiveItem;
            }
            if (item != null)
            {
                listInactive.Remove(item);
                listWorking.Add(item);
                return item;
            }
            else
            {
                item = Instantiate(template);
                ((ObjPooler.IObjPooler)item).SetEvent(OnObjActive, OnObjInactive);
                listWorking.Add(item);
                return item;
            }
        }

        public void InactiveAll()
        {
            var lw = new HashSet<ObjPooler>(listWorking);
            var la = new HashSet<ObjPooler>(listActive);
            foreach (var item in lw)
            {
                item.Inactive();
            }
            foreach (var item in la)
            {
                item.Inactive();
            }
        }

        private void OnObjInactive(ObjPooler obj)
        {
            bool removed = listWorking.Remove(obj);
            removed = removed || listActive.Remove(obj);
            if (removed)
                listInactive.Add(obj);
        }

        private void OnObjActive(ObjPooler obj)
        {
            bool removed = listWorking.Remove(obj);
            removed = removed || listInactive.Remove(obj);
            if (removed)
                listActive.Add(obj);
        }
    }
}