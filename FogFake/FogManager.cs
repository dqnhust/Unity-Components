#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float minFogDistance;
    [SerializeField] private float maxFogDistance;
    [SerializeField] private Color fogColor;
    [SerializeField] private bool previewInEditor;

    [ContextMenu("Set Params")]
    private void SetParams()
    {
        if (mainCamera)
        {
            mainCamera.backgroundColor = fogColor;
        }

        Shader.SetGlobalFloat("_MinFogDistance", minFogDistance);
        Shader.SetGlobalFloat("_MaxFogDistance", maxFogDistance);
        Shader.SetGlobalColor("_FogColor", fogColor);
        Shader.SetGlobalInt("_ApplyFog", Application.isPlaying ? 1 : (previewInEditor ? 1 : 0));
    }

    private void Awake()
    {
        SetParams();
    }

    private void OnValidate()
    {
        SetParams();
    }

    private void OnEnable()
    {
        SetParams();
    }
}