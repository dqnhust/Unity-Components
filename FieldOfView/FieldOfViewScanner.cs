using System.Collections.Generic;
using UnityEngine;

namespace FieldOfView
{
    public interface IFieldOfViewScanner
    {
        /// <summary>
        /// Use World Space
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="totalAngle"></param>
        /// <param name="radius"></param>
        void Show(Vector3 origin, Vector3 direction, float totalAngle, float radius);

        void Hide();
    }

    public class FieldOfViewScanner : MonoBehaviour, IFieldOfViewScanner
    {
        [SerializeField] private InterfaceObject<IFieldOfView> view;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float maxArcPartLength = 0.5f;
        [SerializeField] private int interpolateCount = 10;

        public void Show(Vector3 origin, Vector3 direction, float totalAngle, float radius)
        {
            if (Physics.Linecast(origin + Vector3.up * 4, origin, obstacleLayer))
            {
                view.Value.Hide();
                return;
            }

            view.Value.Show(GetData(origin, direction, totalAngle, radius));
        }

        public void Hide()
        {
            view.Value.Hide();
        }

        /// <summary>
        ///  Use world space
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="totalAngle">Degree</param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Data GetData(Vector3 origin, Vector3 direction, float totalAngle, float radius)
        {
            direction = direction.normalized * radius;
            var rotateAxis = Vector3.up;

            Vector3 GetTargetPoint(float angleFromMinRay)
            {
                var angleFromDirection = angleFromMinRay - totalAngle / 2f;
                return origin + Quaternion.AngleAxis(angleFromDirection, rotateAxis) * direction;
            }

            var result = new Data {origin = origin, targets = new List<Vector3>()};
            var arcLength = 2 * Mathf.PI * radius * totalAngle / 360f;
            var partCount = Mathf.RoundToInt(arcLength / maxArcPartLength);

            bool lastCasted = false;
            Vector3 lastPoint = origin;

            for (int i = 0; i < partCount; i++)
            {
                var angleFromMinRay = totalAngle * i / (partCount - 1);
                var targetPoint = GetTargetPoint(angleFromMinRay);
                var castedTargetPoint = targetPoint;
                var casted = RayCast(origin, targetPoint, ref castedTargetPoint);
                if (i > 0)
                {
                    if (lastCasted != casted)
                    {
                        var (point1, point2) =
                            FindNearestPoint(origin, lastPoint, lastCasted, targetPoint, casted, radius);
                        result.targets.Add(point1);
                        result.targets.Add(point2);
                    }
                }

                result.targets.Add(castedTargetPoint);
                lastCasted = casted;
                lastPoint = castedTargetPoint;
            }

            return result;
        }


        private bool RayCast(Vector3 start, Vector3 end, ref Vector3 point)
        {
            var result = Physics.Linecast(start, end, out var hitInfo, obstacleLayer);
            if (result)
            {
                point = hitInfo.point;
            }

            return result;
        }

        private (Vector3 point1, Vector3 point2) FindNearestPoint(Vector3 origin, Vector3 p1, bool casted1, Vector3 p2,
            bool casted2, float maxDistance)
        {
            Vector3 FindMax(Vector3 p)
            {
                return (p - origin).normalized * maxDistance + origin;
            }

            if (casted1 == casted2)
            {
                return (p1, p2);
            }

            var castedP1 = p1;
            var castedP2 = p2;
            for (int i = 0; i < interpolateCount; i++)
            {
                var center = FindMax(Vector3.Lerp(FindMax(p1), FindMax(p2), 0.5f));
                var castedCenter = center;
                var casted = RayCast(origin, center, ref castedCenter);
                if (casted == casted1)
                {
                    p1 = center;
                    castedP1 = castedCenter;
                }
                else
                {
                    p2 = center;
                    castedP2 = castedCenter;
                }
            }

            return (castedP1, castedP2);
        }
    }
}