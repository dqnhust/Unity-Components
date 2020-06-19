#pragma warning disable 0649
using UnityEngine;

[System.Serializable]
public class FloatRange
{
    public float min;
    public float max;
    public float GetValue() => Random.Range(min, max);
}