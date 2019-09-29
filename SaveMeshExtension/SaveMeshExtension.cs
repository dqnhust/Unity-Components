using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SaveMeshExtension
{
#if UNITY_EDITOR
    [MenuItem("CONTEXT/MeshFilter/Save Mesh")]
    static void SaveMesh(MenuCommand command)
    {
        MeshFilter mf = (MeshFilter)command.context;
        var mesh = Object.Instantiate(mf.sharedMesh);
        var path = EditorUtility.SaveFilePanelInProject("Save Mesh", mf.name, "asset", "Save In?");
        if (string.IsNullOrEmpty(path))
            return;
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.Refresh();
        mf.sharedMesh = mesh;
    }
#endif
}
