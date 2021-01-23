using UnityEngine;

namespace PoolerPack
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ObjPooler : MonoBehaviour, IObjPooler
    {

        public virtual void Inactive()
        {
            gameObject.SetActive(false);
            PoolerStatus = ObjPoolerStatus.Inactive;
        }
        
        public ObjPoolerStatus PoolerStatus { get; set; }

        // ReSharper disable once UnusedMember.Global
        public virtual void Active()
        {
            gameObject.SetActive(true);
            PoolerStatus = ObjPoolerStatus.Active;
        }

    }

    public interface IObjPooler
    {
        // ReSharper disable once UnusedMember.Global
        // void SetEvent(Action<ObjPooler> onActive, Action<ObjPooler> onInActive);

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMemberInSuper.Global
        void Active();

        // ReSharper disable once UnusedMemberInSuper.Global
        void Inactive();

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }

        // ReSharper disable once UnusedMemberInSuper.Global
        // ReSharper disable once UnusedMember.Global
        ObjPoolerStatus PoolerStatus { get; set; }
    }

    public enum ObjPoolerStatus
    {
        DontKnow,
        Active,
        Inactive,
    }
}