using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PoolerPack
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Pooler : IPooler
    {
        private readonly Dictionary<int, PoolerStore> _dict = new Dictionary<int, PoolerStore>();

        public T GetObj<T>(T template) where T : IObjPooler
        {
            if (template == null)
                return default;
            var id = template.gameObject.GetInstanceID();
            var poolerStore = GetPooler(id);
            return (T) poolerStore.GetObj(template);
        }

        public T GetObj<T>(T template, Transform parent) where T : IObjPooler
        {
            if (template == null)
                return default;
            var id = template.gameObject.GetInstanceID();
            var poolerStore = GetPooler(id);
            return (T) poolerStore.GetObj(template, parent);
        }

        public void InActiveAll()
        {
            foreach (var item in _dict)
            {
                item.Value.InactiveAll();
            }
        }

        public PoolerStore GetPooler<T>(T template) where T : IObjPooler
        {
            var id = template.gameObject.GetInstanceID();
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
            private readonly List<IObjPooler> _listObj = new List<IObjPooler>();

            // ReSharper disable once ConvertToAutoPropertyWhenPossible
            // ReSharper disable once UnusedMember.Global
            public IEnumerable<IObjPooler> ListActive =>
                _listObj.Where(obj => obj.PoolerStatus == ObjPoolerStatus.Active);

            public IObjPooler GetObj(IObjPooler template, Transform parent)
            {
                var obj = GetObj(template);
                obj.gameObject.transform.SetParent(parent);
                return obj;
            }

            public IObjPooler GetObj(IObjPooler template)
            {
                if (template == null)
                {
                    return null;
                }

                foreach (var objPooler in _listObj)
                {
                    if (objPooler.PoolerStatus == ObjPoolerStatus.Inactive)
                    {
                        return objPooler;
                    }
                }

                var item = Object.Instantiate(template.gameObject).GetComponent<IObjPooler>();
                item.PoolerStatus = ObjPoolerStatus.DontKnow;
                _listObj.Add(item);
                return item;
            }

            public void InactiveAll()
            {
                foreach (var objPooler in _listObj)
                {
                    if (objPooler.PoolerStatus != ObjPoolerStatus.Inactive)
                    {
                        objPooler.Inactive();
                    }
                }
            }
        }
    }
}