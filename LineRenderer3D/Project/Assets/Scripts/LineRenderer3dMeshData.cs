using System.Collections.Generic;
using UnityEngine;

public class LineRenderer3dMeshData
{
    private float radius;
    private AnimationCurve radiusCurve;
    private Vector3[] centerPoints;
    private Gradient gradientColor;
    /// <summary>
    /// Initializes a new instance of the <see cref="T:LineRenderer3dMeshData"/> class.
    /// </summary>
    /// <param name="radius">Radius.</param>
    /// <param name="radiusCurve">Radius curve.</param>
    /// <param name="smooth">Smooth > 4</param>.</param>
    /// <param name="centerPoints">Center points.</param>
    public LineRenderer3dMeshData(float radius, AnimationCurve radiusCurve, Gradient gradientColor, int smooth, params Vector3[] centerPoints)
    {
        this.radius = radius;
        this.radiusCurve = radiusCurve;
        this.gradientColor = gradientColor;
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

    public Color[] Colors
    {
        get
        {
            return _colors;
        }
    }

    private Vector2[] _uv;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;

    private readonly int pointPerCircle = 10;
    void CalcData()
    {
        CalcVertices();
        CalcTriangles();
        CalcUv();
        CalcColors();
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
        List<Vector2> vs = new List<Vector2>();
        var centerPointLength = centerPoints.Length;
        for (int i = 0; i < centerPointLength - 1; i++)
        {
            float p0 = ((float)i) / (centerPointLength - 1);
            float p1 = ((float)(i + 1f)) / (centerPointLength - 1);
            for (int j = 0; j < pointPerCircle - 1; j++)
            {
                float px = ((float)j) / (pointPerCircle - 1);
                float px1 = ((float)(j + 1)) / (pointPerCircle - 1);
                vs.Add(new Vector2(px, p0));
                vs.Add(new Vector2(px1, p0));
                vs.Add(new Vector2(px1, p1));
                vs.Add(new Vector2(px, p1));
            }
        }
        _uv = vs.ToArray();
    }

    void CalcColors()
    {
        List<Color> cs = new List<Color>();
        var centerPointLength = centerPoints.Length;
        for (int i = 0; i < centerPointLength - 1; i++)
        {
            float p0 = ((float)i) / (centerPointLength - 1);
            float p1 = ((float)(i + 1f)) / (centerPointLength - 1);
            for (int j = 0; j < pointPerCircle - 1; j++)
            {
                cs.Add(gradientColor.Evaluate(p0));
                cs.Add(gradientColor.Evaluate(p0));
                cs.Add(gradientColor.Evaluate(p1));
                cs.Add(gradientColor.Evaluate(p1));
            }
        }
        _colors = cs.ToArray();
    }
}
