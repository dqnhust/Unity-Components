using UnityEngine;

namespace PoolerPack
{
    public interface IPooler
    {
        T GetObj<T>(T template) where T : ObjPooler;
        T GetObj<T>(T template, Transform parent) where T : ObjPooler;
        void InActiveAll();
        Pooler.PoolerStore GetPooler<T>(T template) where T : ObjPooler;
    }
}