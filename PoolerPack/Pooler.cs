using System.Collections.Generic;
using UnityEngine;

namespace PoolerPack
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Pooler : IPooler
    {
        private readonly Dictionary<int, PoolerStore> _dict = new Dictionary<int, PoolerStore>();

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
            foreach (var item in _dict)
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
            if (!_dict.TryGetValue(id, out var pooler))
            {
                pooler = new PoolerStore();
                _dict.Add(id, pooler);
            }
            return pooler;
        }

        public class PoolerStore
        {
            private readonly HashSet<ObjPooler> _listInactive = new HashSet<ObjPooler>();
            private readonly HashSet<ObjPooler> _listWorking = new HashSet<ObjPooler>();
            private readonly HashSet<ObjPooler> _listActive = new HashSet<ObjPooler>();

            // ReSharper disable once ConvertToAutoPropertyWhenPossible
            // ReSharper disable once UnusedMember.Global
            public HashSet<ObjPooler> ListActive => _listActive;

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
                foreach (var inactiveItem in _listInactive)
                {
                    item = inactiveItem;
                }
                if (item != null)
                {
                    _listInactive.Remove(item);
                    _listWorking.Add(item);
                    return item;
                }
                else
                {
                    item = Object.Instantiate(template);
                    ((ObjPooler.IObjPooler)item).SetEvent(OnObjActive, OnObjInactive);
                    _listWorking.Add(item);
                    return item;
                }
            }

            public void InactiveAll()
            {
                var lw = new HashSet<ObjPooler>(_listWorking);
                var la = new HashSet<ObjPooler>(_listActive);
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
                bool removed = _listWorking.Remove(obj);
                removed = removed || _listActive.Remove(obj);
                if (removed)
                    _listInactive.Add(obj);
            }

            private void OnObjActive(ObjPooler obj)
            {
                bool removed = _listWorking.Remove(obj);
                removed = removed || _listInactive.Remove(obj);
                if (removed)
                    _listActive.Add(obj);
            }
        }
    }
}