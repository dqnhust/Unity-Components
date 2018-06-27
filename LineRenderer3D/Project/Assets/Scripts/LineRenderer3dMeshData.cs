using System.Collections.Generic;
using UnityEngine;

public class LineRenderer3dMeshData
{
    private float radius;
    private AnimationCurve radiusCurve;
    private Vector3[] centerPoints;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:LineRenderer3dMeshData"/> class.
    /// </summary>
    /// <param name="radius">Radius.</param>
    /// <param name="radiusCurve">Radius curve.</param>
    /// <param name="smooth">Smooth > 4</param>.</param>
    /// <param name="centerPoints">Center points.</param>
    public LineRenderer3dMeshData(float radius, AnimationCurve radiusCurve, int smooth, params Vector3[] centerPoints)
    {
        this.radius = radius;
        this.radiusCurve = radiusCurve;
        this.centerPoints = centerPoints;
        this.pointPerCircle = smooth;
        if (centerPoints.Length < 2)
        {
            throw new System.Exception("Cannot Calculate LineRender3D with " + centerPoints.Length + " point.");
        }
        CalcData();
    }

    public Vector3[] Vertices
    {
        get
        {
            return _vertices;
        }
    }

    public int[] Triangles
    {
        get
        {
            return _triangles;
        }
    }

    public Vector2[] Uv
    {
        get
        {
            return _uv;
        }
    }

    private Vector2[] _uv;
    private Vector3[] _vertices;
    private int[] _triangles;
    private readonly int pointPerCircle = 10;
    void CalcData()
    {
        CalcVertices();
        CalcTriangles();
        CalcUv();
    }

    void CalcVertices()
    {
        List<Vector3[]> circleVert = new List<Vector3[]>();
        var centerPointLength = centerPoints.Length;
        for (int i = 0; i < centerPointLength; i++)
        {
            Vector3 vectorN;
            if (i == 0)
            {
                vectorN = centerPoints[0] - centerPoints[1];
            }
            else
            {
                vectorN = centerPoints[i - 1] - centerPoints[i];
            }
            var p = ((float)i) / (centerPointLength - 1);
            var r = radiusCurve.Evaluate(p) * this.radius;
            var center = centerPoints[i];
            var x = new Circle3DVertices(center, vectorN, r, pointPerCircle);
            circleVert.Add(x.Vertices);
        }
        var circleLength = circleVert.Count;
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < circleLength - 1; i++)
        {
            var cc = circleVert[i];
            var ac = circleVert[i + 1];
            for (int j = 0; j < pointPerCircle - 1; j++)
            {
                verts.Add(cc[j]);
                verts.Add(cc[j + 1]);
                verts.Add(ac[j + 1]);
                verts.Add(ac[j]);
            }
        }
        _vertices = verts.ToArray();
    }

    void CalcTriangles()
    {
        var vertLength = _vertices.Length;
        var trisLength = (int)(vertLength * 1.5f);
        _triangles = new int[trisLength];
        int[] arr = new int[] {
            0, 2, 1, 0, 3, 2
        };
        for (int i = 0; i < trisLength; i++)
        {
            int f = Mathf.FloorToInt(((float)i) / 6f) * 4;
            int j = i % 6;
            _triangles[i] = f + arr[j];
        }
    }

    void CalcUv()
    {
        var vertLength = _vertices.Length;
        _uv = new Vector2[vertLength];
        for (int i = 0; i < vertLength; i++)
        {
            var v = _vertices[i];
            _uv[i] = new Vector2(v.x + v.z, v.y + v.z);
        }
    }
}
