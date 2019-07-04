#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineView : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float width = 2;

    private void Update()
    {
        if (meshFilter == null) return;
        var mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            mesh = meshFilter.sharedMesh = new Mesh();
        }
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i).localPosition);
        }
        if (points.Count == 0)
            return;
        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float totalDistance = 0;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 p0;
            var p1 = points[i];
            Vector3 p2;
            if (i > 0)
            {
                p0 = points[i - 1];
            }
            else
            {
                p0 = p1 + (p1 - points[i + 1]);
            }
            if (i < points.Count - 1)
            {
                p2 = points[i + 1];
            }
            else
            {
                p2 = p1 + (p1 - p0).normalized;
            }
            totalDistance += Vector3.Distance(p1, p2);
            var bis = Bisectrix(p0 - p1, p2 - p1, Vector3.up);

            var halfWidth = width / 2f;
            Debug.Log(halfWidth);
            halfWidth = halfWidth / Mathf.Sin(Vector3.Angle(bis, p1 - p0) * Mathf.Deg2Rad);
            Debug.Log("=>>" + halfWidth);
            var px = p1 + bis * halfWidth;
            var py = p1 - bis * halfWidth;

            var vertOffset = vertices.Count;
            vertices.Add(px);
            vertices.Add(py);

            if (vertOffset >= 2)
            {
                tris.Add(vertOffset + -2);
                tris.Add(vertOffset + 0);
                tris.Add(vertOffset + -1);

                tris.Add(vertOffset + -1);
                tris.Add(vertOffset + 0);
                tris.Add(vertOffset + 1);
            }

            uvs.Add(new Vector2(0, totalDistance));
            uvs.Add(new Vector2(1, totalDistance));
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(tris, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    private Vector3 Bisectrix(Vector3 v1, Vector3 v2, Vector3 axis)
    {
        var signAngle = Vector3.SignedAngle(v1, v2, axis);
        signAngle *= 0.5f;
        float sign = Mathf.Sign(signAngle);
        return sign * (Quaternion.AngleAxis(signAngle, axis) * v1).normalized;
    }

    //private void OnDrawGizmos()
    //{
    //    for (int i = 0, j = 1; j < transform.childCount; i++, j++)
    //    {
    //        Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
    //    }
    //    int ii = 0;
    //    foreach (var item in meshFilter.sharedMesh.vertices)
    //    {
    //        UnityEditor.Handles.Label(item, ii + "->" + item.ToString());
    //        Gizmos.DrawSphere(item, 0.1f);
    //        ii++;
    //    }
    //}
}
