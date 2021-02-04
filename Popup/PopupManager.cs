#pragma warning disable 0649
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ui.Popup
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private Popup defaultPopup;
        [SerializeField] private PopupEvent popupEvent;

        private readonly List<Popup> _listPopup = new List<Popup>();

        public T GetPopup<T>() where T : Popup
        {
            if (!_init)
            {
                Init();
            }

            foreach (var popup in _listPopup)
            {
                if (popup is T instance)
                {
                    return instance;
                }
            }

            Debug.LogError("Cannot Find Popup" + (typeof(T).Name));
            return null;
        }

        public Popup GetPopup(GameObject template)
        {
            if (!_init)
            {
                Init();
            }

            foreach (var popup in _listPopup)
            {
                if (popup.gameObject.name == template.name)
                {
                    return popup;
                }
            }

            return GeneratePopup(template.GetComponent<Popup>());
            //throw new UnityException($"Cannot Find Popup Have Prefab : {template.name}");
        }

        private Popup GeneratePopup(Popup popup)
        {
            if (popup == null)
            {
                return null;
            }
            var result = Instantiate(popup, transform);
            result.gameObject.name = popup.gameObject.name;
            result.transform.localScale = Vector3.one;
            result.Init();
            result.Close();
            return result;
        }

        bool _init;

        public void Init()
        {
            if (_init)
                return;
            _init = true;
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var p = transform.GetChild(i).GetComponent<Popup>();
                if (p != null)
                {
                    _listPopup.Add(p);
                    p.Init();
                }
            }

            foreach (var popup in _listPopup)
            {
                popup.Close();
            }

            if (defaultPopup != null)
                defaultPopup.Open();
            popupEvent.OnCallShowPopup += PopupEventOnOnCallShowPopup;
        }

        private void OnDestroy()
        {
            popupEvent.OnCallShowPopup -= PopupEventOnOnCallShowPopup;
        }

        private void PopupEventOnOnCallShowPopup(GameObject popupPrefab)
        {
            GetPopup(popupPrefab).Open();
        }

        private void Awake()
        {
            Init();
        }
    }
}