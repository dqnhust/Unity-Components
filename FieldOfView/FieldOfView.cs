using System.Collections.Generic;
using FieldOfView;
using UnityEngine;

namespace DqnAsset.FieldOfView
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
            data.origin = transform.InverseTransformPoint(data.origin);
            for (int i = 0; i < data.targets.Count; i++)
            {
                data.targets[i] = transform.InverseTransformPoint(data.targets[i]);
            }

            Init();

            _vertices.Clear();
            _vertices.Add(data.origin);
            _vertices.AddRange(data.targets);
            var verticesCount = _vertices.Count;

            _uvs.Clear();
            _uvs.Add(Vector2.zero);
            // var defaultUv = Vector2.zero;
            foreach (var position in data.targets)
            {
                var distanceWithOrigin = Vector3.Distance(data.origin, position);
                var percent = distanceWithOrigin / data.maxDistance;
                percent = Mathf.Clamp01(percent);
                _uvs.Add(new Vector2(0, percent));
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