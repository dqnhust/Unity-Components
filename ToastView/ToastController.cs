#pragma warning disable 0649
using GameEvent;
using UnityEngine;

namespace ToastView
{
    public class ToastController : MonoBehaviour
    {
        [SerializeField] private GameManagerEvent gameManagerEvent;
        [SerializeField] private ToastView toastView;

        private void OnEnable()
        {
            gameManagerEvent.EventShowToast += GameManagerEventOnEventShowToast;
        }

        private void OnDisable()
        {
            gameManagerEvent.EventShowToast -= GameManagerEventOnEventShowToast;
        }

        private void GameManagerEventOnEventShowToast(string message)
        {
            toastView.ShowToast(message);
        }
    }
}