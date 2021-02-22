using UnityEngine;

namespace FieldOfView
{
    public class TestFieldScanner : MonoBehaviour
    {
        [SerializeField] private InterfaceObject<FieldOfViewScanner> target;
        [SerializeField] private Transform testTransform;
        [SerializeField] private float radius;

        private void LateUpdate()
        {
            target.Value.Show(testTransform.position, Vector3.forward, 360f, radius);
        }
    }
}