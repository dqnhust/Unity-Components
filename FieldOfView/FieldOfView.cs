using System;
using System.Collections.Generic;
using UnityEngine;

namespace FieldOfView
{
    public interface IFieldOfView
    {
        void Show(Data data);
        void Hide();
    }

    public class FieldOfView : MonoBehaviour, IFieldOfView
    {
        [SerializeField] private MeshFilter meshFilter;

        private Mesh _mesh;
        private List<Vector3> _vertices;
        private List<Vector2> _uvs;
        private List<int> _triangles;
        private List<Vector3> _normals;

        public void Show(Data data)
        {
            Init();

            _vertices.Clear();
            _vertices.Add(data.origin);
            _vertices.AddRange(data.targets);
            var verticesCount = _vertices.Count;

            _uvs.Clear();
            var defaultUv = Vector2.zero;
            for (var i = 0; i < verticesCount; i++)
            {
                _uvs.Add(defaultUv);
            }

            _triangles.Clear();
            for (int i = 1, j = 2; j < verticesCount; i++, j++)
            {
                _triangles.Add(0);
                _triangles.Add(i);
                _triangles.Add(j);
            }

            _normals.Clear();
            var defaultNormal = Vector3.up;
            for (int i = 0; i < verticesCount; i++)
            {
                _normals.Add(defaultNormal);
            }

            ApplyMesh();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Init()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _vertices = new List<Vector3>();
                _uvs = new List<Vector2>();
                _triangles = new List<int>();
                _normals = new List<Vector3>();
            }

            _mesh.Clear();
            meshFilter.sharedMesh = _mesh;
        }

        private void ApplyMesh()
        {
            _mesh.MarkDynamic();
            _mesh.Clear();
            _mesh.SetVertices(_vertices);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.SetNormals(_normals);
            _mesh.SetUVs(0, _uvs);
            _mesh.RecalculateBounds();
        }

        private void OnDestroy()
        {
            Destroy(_mesh);
        }
    }
}