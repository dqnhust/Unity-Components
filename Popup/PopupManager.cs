#pragma warning disable 0649
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popup
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private Popup defaultPopup;

        private List<Popup> listPopup = new List<Popup>();

        public T GetPopup<T>() where T : Popup
        {
            if (!init)
            {
                Init();
            }

            foreach (var popup in listPopup)
            {
                if (popup is T)
                {
                    return (T) popup;
                }
            }

            throw new UnityException("Cannot Find Popup" + (typeof(T).Name));
        }

        bool init = false;

        public void Init()
        {
            if (init)
                return;
            init = true;
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Popup p = transform.GetChild(i).GetComponent<Popup>();
                if (p != null)
                {
                    listPopup.Add(p);
                    p.Init();
                }
            }

            foreach (var popup in listPopup)
            {
                popup.Close();
            }

            if (defaultPopup != null)
                defaultPopup.Open();
        }

        private void Awake()
        {
            Init();
        }
    }
}