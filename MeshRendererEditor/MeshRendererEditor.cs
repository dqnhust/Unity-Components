using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererEditor : Editor
{
    private string[] sortingLayerNames;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MeshRenderer script = (MeshRenderer)target;
        int currentIndex = System.Array.IndexOf(sortingLayerNames, script.sortingLayerName);
        currentIndex = EditorGUILayout.Popup("Sorting Layer ", currentIndex, sortingLayerNames);
        script.sortingLayerName = sortingLayerNames[currentIndex];
        script.sortingOrder = EditorGUILayout.IntField("Sorting Order ", script.sortingOrder);
    }

    void OnEnable()
    {
        //MeshRenderer script = (MeshRenderer)target;
        //script.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //script.receiveShadows = false;
        //script.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        //script.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        //script.probeAnchor = null;
        //script.lightProbeProxyVolumeOverride = null;

        System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        sortingLayerNames = (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}