using UnityEngine;

namespace PoolerPack
{
    public interface IPooler
    {
        T GetObj<T>(T template) where T : IObjPooler;
        T GetObj<T>(T template, Transform parent) where T : IObjPooler;
        void InActiveAll();
        Pooler.PoolerStore GetPooler<T>(T template) where T : IObjPooler;
    }
}