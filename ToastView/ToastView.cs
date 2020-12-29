#pragma warning disable 0649
using System;
using PoolerPack;
using UnityEngine;

namespace ToastView
{
    public class ToastView : MonoBehaviour
    {
        [SerializeField] private ToastMessageView toastMessageView;

        private Pooler _pooler;

        public void ShowToast(string message)
        {
            if (_pooler == null)
            {
                _pooler = new Pooler();
            }

            var obj = _pooler.GetObj(toastMessageView, toastMessageView.transform.parent);
            obj.transform.localScale = Vector3.one;
            obj.Setup(message);
            obj.transform.localPosition = toastMessageView.transform.localPosition;
        }
    }
}