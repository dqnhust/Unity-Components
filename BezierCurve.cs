using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    private readonly Vector3[] _points;

    public BezierCurve(params Vector3[] points)
    {
        this._points = points;
    }

    public Vector3 GetPoint(float percent)
    {
        var result = new List<Vector3>(_points);
        while (result.Count > 1)
        {
            var n = result.Count;
            for (int i = 0, j = 1; j < n; i++, j++)
            {
                result.Add(Vector3.Lerp(result[i], result[j], percent));
            }

            result.RemoveRange(0, n);
        }

        return result[0];
    }
}