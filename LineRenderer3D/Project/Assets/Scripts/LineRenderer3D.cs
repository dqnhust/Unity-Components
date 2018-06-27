using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Effects/LineRenderer3D")]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public partial class LineRenderer3D : MonoBehaviour
{
    [SerializeField] private float radius = 1;
    [Range(4, 300)]
    [SerializeField] private int smooth;
    [SerializeField] private AnimationCurve radiusCurve;
    [SerializeField] private Vector3[] points;

    public void SetRadius(float radius)
    {
        this.radius = radius;
    }

    public void SetCurve(AnimationCurve animationCurve)
    {
        radiusCurve = animationCurve;
    }

    public void SetPoints(Vector3[] points)
    {
        this.points = points;
    }

    public void SetPoint(int index, Vector3 point)
    {
        points[index] = point;
    }


    /// <summary>
    /// Set the PointCount also Reset Points Array.
    /// </summary>
    /// <value>The point Count.</value>
    public int PointCount
    {
        get
        {
            return points.Length;
        }
        set
        {
            points = new Vector3[value];
        }
    }

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void UpdateView()
    {
        AssignComponent();
        RenderMesh();
    }

    private void AssignComponent()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            meshFilter.sharedMesh = mesh;
        }
    }

    private void RenderMesh()
    {
        var data = new LineRenderer3dMeshData(radius, radiusCurve, smooth, points);
        mesh.Clear();
        if (points.Length < 2) return;
        mesh.vertices = data.Vertices;
        mesh.triangles = data.Triangles;
        mesh.uv = data.Uv;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }


    private void OnValidate()
    {
        UpdateView();
    }
}
