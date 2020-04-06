#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FogFake : MonoBehaviour
{
    [SerializeField] private Vector2 quadSize;
    [SerializeField] private float length;
    [SerializeField] private float space;
    [SerializeField] private bool createOnAwake;

    private void Awake()
    {
        if (createOnAwake)
            CreateFog();
    }

    [ContextMenu("Create Fog")]
    public void CreateFog() => CreateFog(quadSize, length, space);

    public void CreateFog(Vector2 quadSize, float length, float space)
    {
        var mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.sharedMesh;
        if (mesh == null)
        {
            mf.sharedMesh = mesh = new Mesh();
        }
        mesh.Clear();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector3 min = -quadSize / 2f;
        Vector3 max = quadSize / 2f;
        Vector3 p1 = min;
        Vector3 p2 = new Vector3(min.x, max.y);
        Vector3 p3 = max;
        Vector3 p4 = new Vector3(max.x, min.y);

        for (float z = 0; z < length; z += space)
        {
            int i = vertices.Count;
            vertices.Add(p1 + Vector3.forward * z);
            vertices.Add(p2 + Vector3.forward * z);
            vertices.Add(p3 + Vector3.forward * z);
            vertices.Add(p4 + Vector3.forward * z);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));

            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);

            triangles.Add(i + 0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
            triangles.Add(i + 0);
            triangles.Add(i + 2);
            triangles.Add(i + 3);
        }

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.RecalculateBounds();

        var mat = GetComponent<MeshRenderer>().sharedMaterial;
        var distanceFromCam = DistanceFromCamera;
        mat.SetFloat("_MinDistanceFog", distanceFromCam);
        mat.SetFloat("_MaxDistanceFog", distanceFromCam + length);
    }

    private float DistanceFromCamera
    {
        get
        {
            var cam = Camera.allCameras[0];
            return Vector3.Distance(cam.transform.position, transform.position);
        }
    }
}
