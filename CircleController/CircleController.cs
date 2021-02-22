using UnityEngine;

namespace CircleController
{
    public class CircleController : MonoBehaviour, ICircleController
    {
        [SerializeField] private GameObject view;
        [SerializeField] private Transform innerView;
        [SerializeField] private Camera cameraView;

        public Vector2 Direction { get; private set; } = Vector2.zero;
        private float Radius => 0.5f * GetComponent<RectTransform>().rect.size.x * transform.localScale.x;

        private void OnDisable()
        {
            Hide();
        }

        private Vector3 LocalMousePosition
        {
            get
            {
                var r = transform.parent.InverseTransformPoint(cameraView.ScreenToWorldPoint(Input.mousePosition));
                r.z = 0;
                return r;
            }
        }

        private Vector3 _pressMousePosition;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Show(LocalMousePosition);
                _pressMousePosition = LocalMousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                var delta = LocalMousePosition - _pressMousePosition;
                if (delta.magnitude > Radius)
                {
                    delta = delta.normalized * Radius;
                }

                innerView.localPosition = delta;
                Direction = delta;
            }
            else
            {
                Hide();
            }
        }

        private void Show(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            view.SetActive(true);
        }

        private void Hide()
        {
            view.SetActive(false);
        }
    }
}