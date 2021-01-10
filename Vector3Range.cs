using UnityEngine;

[System.Serializable]
public class Vector3Range
{
    public Vector3 min;
    public Vector3 max;

    public Vector3 GetValue() =>
        new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
}