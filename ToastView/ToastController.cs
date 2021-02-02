#pragma warning disable 0649
using UnityEngine;

namespace ToastView
{
    public class ToastController : MonoBehaviour
    {
        [SerializeField] private ToastEvent toastEvent;
        [SerializeField] private ToastView toastView;

        private void OnEnable()
        {
            toastEvent.EventShowToast += OnEventShowToast;
        }

        private void OnDisable()
        {
            toastEvent.EventShowToast -= OnEventShowToast;
        }

        private void OnEventShowToast(string message)
        {
            toastView.ShowToast(message);
        }
    }
}