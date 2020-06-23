using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapView))]
public class MapViewEditor : Editor
{
    private MapView Script => target as MapView;
    private string selectingNode;
    private readonly float selectionRadius = 1f;
    private readonly Color colorLinkedNode = Color.green;
    private List<string> wayShowing;

    private void OnEnable()
    {
        wayShowing = new List<string>();
        selectingNode = "";
    }

    private Vector3 IntersectionOfLineAndPlane(Vector3 linePoint, Vector3 lineDireciton, Vector3 planePoint, Vector3 planeNormal)
    {
        return linePoint - lineDireciton * Vector3.Dot(linePoint - planePoint, planeNormal) / Vector3.Dot(lineDireciton, planeNormal);
    }

    private void OnSceneGUI()
    {
        var handlesColorDefault = Handles.color;
        var cam = SceneView.currentDrawingSceneView.camera;
        var e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector3 mousePos = e.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = cam.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Ray ray = cam.ScreenPointToRay(mousePos);
            var mouseWorldPos = IntersectionOfLineAndPlane(ray.origin, ray.direction, Vector3.zero, Vector3.up);
            float minD = float.MaxValue;
            MapView.MapNode selectingItem = null;

            foreach (var item in Script.nodes)
            {
                var d = Vector3.Distance(item.position, mouseWorldPos);
                if (d < selectionRadius)
                {
                    if (selectingItem == null || d < minD)
                    {
                        selectingItem = item;
                        minD = d;
                    }
                }
            }
            if (selectingItem != null && selectingItem.id != selectingNode)
            {
                //Show Way To Reach Node
                if (e.control && !string.IsNullOrEmpty(selectingNode))
                {
                    ShowWay(selectingNode, selectingItem.id);
                }
                else
                {
                    wayShowing = null;
                }
                if (e.shift && !string.IsNullOrEmpty(selectingNode))
                {
                    bool isAdd;
                    AddOrRemoveLink(selectingItem, GetNodeById(selectingNode), out isAdd);
                    if (isAdd)
                    {
                        selectingNode = selectingItem.id;
                    }
                }
                else
                {
                    selectingNode = selectingItem.id;
                }
                e.Use();
            }
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
        {
            RemoveNode(selectingNode);
            e.Use();
        }

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        if (cam.orthographic)
        {
            var p = cam.ScreenToWorldPoint(Vector3.one);
            style.fontSize = Mathf.RoundToInt(500 * selectionRadius / cam.orthographicSize);
        }
        else
        {
            var size = cam.WorldToScreenPoint(Vector3.one).magnitude / Vector3.one.magnitude;
            style.fontSize = Mathf.RoundToInt(selectionRadius * size / 30f);
        }
        foreach (var node in Script.nodes)
        {
            if (selectingNode == node.id)
            {
                var pos = Handles.PositionHandle(node.position, Quaternion.identity);
                if (pos != node.position)
                {
                    Undo.RecordObject(target, "Move Node");
                    node.position = pos;
                }
            }
        }
        var nodeIndex = 0;
        foreach (var node in Script.nodes)
        {
            Handles.color = handlesColorDefault;
            if (!string.IsNullOrEmpty(selectingNode))
            {
                if (CheckLinkExist(selectingNode, node.id))
                {
                    Handles.color = colorLinkedNode;
                }

            }
            //Draw Nodes
            Handles.DrawSolidDisc(node.position, Vector3.up, selectionRadius);
            {
                Handles.BeginGUI();
                var pos = HandleUtility.WorldToGUIPoint(node.position);
                GUI.Label(new Rect(pos.x - 5, pos.y - 5, 10, 10), nodeIndex.ToString(), style);
                Handles.EndGUI();
            }
            nodeIndex++;
        }
        Handles.color = handlesColorDefault;
        foreach (var link in Script.links)
        {
            var node1 = GetNodeById(link.id1);
            var node2 = GetNodeById(link.id2);
            Handles.DrawLine(node1.position, node2.position);
        }
        Handles.DrawWireCube(Script.Center, Script.Size3D);

        //ShowWay
        if (wayShowing != null)
        {
            Handles.color = Color.red;
            for (int i = 0; i < wayShowing.Count - 1; i++)
            {
                var item0 = wayShowing[i];
                var item1 = wayShowing[i + 1];
                Handles.DrawLine(GetNodeById(item0).position, GetNodeById(item1).position);
            }
            Handles.color = handlesColorDefault;
        }
    }

    private void ShowWay(string from, string to)
    {
        wayShowing = Script.GetWayFromNode(from, to);
    }

    private MapView.MapNode GetNodeById(string id)
    {
        foreach (var item in Script.nodes)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }

    private void AddOrRemoveLink(MapView.MapNode node1, MapView.MapNode node2, out bool isAdd)
    {
        if (CheckLinkExist(node1.id, node2.id))
        {
            isAdd = false;
            RemoveLink(node1, node2);
        }
        else
        {
            isAdd = true;
            AddLink(node1, node2);
        }
    }

    private void AddLink(MapView.MapNode node1, MapView.MapNode node2)
    {
        if (node1.id == node2.id)
        {
            Debug.Log("Cannot Link Same Node!");
            return;
        }
        if (CheckLinkExist(node1.id, node2.id))
        {
            Debug.Log("Cannot Add Existed Link!");
            return;
        }
        Undo.RecordObject(target, "Add Link!");
        Script.links.Add(new MapView.MapNodeLink(node1.id, node2.id));
    }

    private void RemoveLink(MapView.MapNode node1, MapView.MapNode node2)
    {
        if (node1.id == node2.id)
        {
            Debug.Log("Cannot Remove Link Same Node!");
            return;
        }
        if (!CheckLinkExist(node1.id, node2.id))
        {
            Debug.Log("Cannot Remove not Exist Link!");
            return;
        }
        Undo.RecordObject(target, "Remove Link!");
        Script.links.RemoveAll((link) =>
        {
            bool b = link.Compare(node1.id, node2.id);
            if (b)
            {
                Debug.Log("Removed Link!");
            }
            return b;
        });
    }

    private void RemoveNode(string nodeId)
    {
        if (string.IsNullOrEmpty(nodeId))
            return;
        Script.nodes.RemoveAll((node) =>
        {
            return nodeId == node.id;
        });
        Script.links.RemoveAll((link) =>
        {
            return link.id1 == nodeId || link.id2 == nodeId;
        });
    }

    private bool CheckLinkExist(string nodeId1, string nodeId2)
    {
        foreach (var link in Script.links)
        {
            if (link.Compare(nodeId1, nodeId2))
                return true;
        }
        return false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Label("Node Count:" + Script.nodes.Count);
        GUILayout.Label("Link Count:" + Script.links.Count);
        var style = new GUIStyle();
        style.fontStyle = FontStyle.BoldAndItalic;

        GUILayout.Label("Hold Shift And Select\nTwo Node To Link/Remove Link it!", style);
        GUILayout.Label("Hold Control And Select\nTwo Node To Show Way To Connect It!", style);

        if (GUILayout.Button("Add Node"))
        {
            Undo.RecordObject(target, "Add Node");
            var newNode = new MapView.MapNode(Vector3.zero);
            Script.nodes.Add(newNode);
            foreach (var item in SceneView.sceneViews)
            {
                (item as SceneView).Repaint();
            }
            selectingNode = newNode.id;
        }
    }
}
