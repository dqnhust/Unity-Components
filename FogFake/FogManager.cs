#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private float minFogDistance;
    [SerializeField] private float maxFogDistance;
    [SerializeField] private Color fogColor;

    [ContextMenu("Set Params")]
    private void SetParams()
    {
        Shader.SetGlobalFloat("_MinFogDistance", minFogDistance);
        Shader.SetGlobalFloat("_MaxFogDistance", maxFogDistance);
        Shader.SetGlobalColor("_FogColor", fogColor);
    }

    private void Awake()
    {
        SetParams();
    }

    private void OnValidate()
    {
        SetParams();
    }
}
