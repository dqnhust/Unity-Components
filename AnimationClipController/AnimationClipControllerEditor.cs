using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationClipController))]
public class AnimationClipControllerEditor : Editor
{
    private float time;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        var script = target as AnimationClipController;
        time = GUILayout.HorizontalSlider(time, 0, script.ClipLength, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        if (EditorGUI.EndChangeCheck())
        {
            script.ClipTime = time;
        }
        GUILayout.Label($"Clip Time:{script.ClipTime}/{script.ClipLength}");
    }
}
