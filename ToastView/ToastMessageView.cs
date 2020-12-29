#pragma warning disable 0649
using System.Collections;
using PoolerPack;
using UnityEngine;
using UnityEngine.UI;

namespace ToastView
{
    public class ToastMessageView : ObjPooler
    {
        [SerializeField] private Text textView;
        [SerializeField] private float showTime;

        public void Setup(string message)
        {
            textView.text = message;
            Active();
            StartCoroutine(IeAutoHide());
        }

        private IEnumerator IeAutoHide()
        {
            yield return new WaitForSeconds(showTime);
            Inactive();
        }
    }
}