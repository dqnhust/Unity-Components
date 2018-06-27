using UnityEngine;

public class Circle3DVertices
{
    private Vector3 center;
    private Vector3 normal;
    private float radius;
    private int pointCount;
    public Circle3DVertices(Vector3 center, Vector3 normal, float radius, int pointCount)
    {
        this.center = center;
        this.radius = radius;
        this.pointCount = pointCount;
        this.normal = normal.normalized;
        CalcData();
    }

    public Vector3[] Vertices
    {
        get
        {
            return _vertices;
        }
    }

    private Vector3[] _vertices;

    private void CalcData()
    {
        _vertices = new Vector3[pointCount];
        var vectorX = new Vector3(normal.x + 1, normal.y + 1, normal.z).normalized;
        var v0 = center + Vector3.Cross(normal, vectorX).normalized * radius;
        for (int i = 0; i < pointCount; i++)
        {
            var angle = ((float)i / (pointCount - 1f)) * 360;
            _vertices[i] = Quaternion.AngleAxis(angle, normal) * (v0 - center) + center;
        }
    }
}