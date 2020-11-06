#pragma warning disable 0649
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

#if UNITY_EDITOR
// [InitializeOnLoad]
#endif
public class CameraFixAspect : MonoBehaviour
{
// #if UNITY_EDITOR
//     // private static CameraFixAspect[] instances;
//
//    //  static CameraFixAspect()
//    //  {
//    //      instances = null;
//    //      EditorApplication.update -= OnEditorUpdate;
//    //      EditorApplication.update += OnEditorUpdate;
//    // }
//    //
//    //  private void OnDestroy()
//    //  {
//    //      EditorApplication.update -= OnEditorUpdate;
//    //  }
//
//     // private static void OnEditorUpdate()
//     // {
//     //     if (instances == null)
//     //     {
//     //         instances = FindObjectsOfType<CameraFixAspect>();
//     //     }
//     //
//     //     if (instances != null)
//     //     {
//     //         foreach (var item in instances)
//     //         {
//     //             if (item == null)
//     //             {
//     //                 continue;
//     //             }
//     //             item.UpdateView();
//     //         }
//     //     }
//     // }
// #endif


    public enum FixDirection
    {
        Horizontal,
        Vertical
    }

    [SerializeField] private Camera cam;
    [SerializeField] private FixDirection fixDirection;
    [SerializeField] private float length;

    private void OnEnable()
    {
        UpdateView();
    }

    // private void OnValidate()
    // {
    //     UpdateView();
    // }

    [ContextMenu("Update View")]
    private void UpdateView()
    {
        switch (fixDirection)
        {
            case FixDirection.Horizontal:
                var height = length / cam.aspect;
                cam.orthographicSize = height * 0.5f;
                break;
            case FixDirection.Vertical:
                cam.orthographicSize = length / 2f;
                break;
            default:
                return;
        }
    }
}