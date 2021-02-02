using UnityEngine;

namespace Ui.Popup
{
    public abstract class Popup : MonoBehaviour
    {
        [ContextMenu("Open")]
        public virtual void Open()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
            OnOpen();
        }

        [ContextMenu("Close")]
        public virtual void Close()
        {
            OnClose();
            gameObject.SetActive(false);
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }

        public virtual void Init()
        {
        }
    }
}