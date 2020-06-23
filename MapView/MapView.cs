#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapView : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;

    public Vector3 Size3D => new Vector3(size.x, 0, size.y);
    private Vector3 Offset3D => new Vector3(offset.x, 0, offset.y);
    public Vector3 Min
    {
        get => transform.position + Offset3D - Size3D / 2f;
    }

    public Vector3 Max
    {
        get => transform.position + Offset3D + Size3D / 2f;
    }

    public Vector3 Center => transform.position + Offset3D;

    public Vector3 Clamp(Vector3 pos)
    {
        return new Vector3(Mathf.Clamp(pos.x, Min.x, Max.x), 0, Mathf.Clamp(pos.z, Min.z, Max.z));
    }

    #region Nodes and Links
    public List<MapNode> nodes;
    public List<MapNodeLink> links;
    public string NameOfNodes => nameof(nodes);
    public string NameOfLinks => nameof(links);

    public List<string> GetLinkedNode(string nodeId)
    {
        List<string> l = new List<string>();
        foreach (var link in links)
        {
            string otherId = "";
            if (link.Contain(nodeId, out otherId))
            {
                l.Add(otherId);
            }
        }
        return l;
    }

    [System.Serializable]
    public class MapNode
    {
        public string id;
        public Vector3 position;

        public MapNode(Vector3 position)
        {
            this.position = position;
            this.id = System.Guid.NewGuid().ToString();
        }
    }

    [System.Serializable]
    public class MapNodeLink
    {
        public string id1;
        public string id2;

        public MapNodeLink(string id1, string id2)
        {
            this.id1 = id1;
            this.id2 = id2;
        }

        public bool Contain(string id, out string otherId)
        {
            if (id == id1)
            {
                otherId = id2;
                return true;
            }
            else if (id == id2)
            {
                otherId = id1;
                return true;
            }
            otherId = "";
            return false;
        }

        public bool Compare(string id1, string id2)
        {
            if (id1 == this.id1 && id2 == this.id2)
                return true;
            if (id1 == this.id2 && id2 == this.id1)
                return true;
            return false;
        }
    }

    public List<string> GetWayFromNode(string nodeId1, string nodeId2)
    {
        Dictionary<string, string> traceDict = new Dictionary<string, string>();

        HashSet<string> willCalculate = new HashSet<string>();
        HashSet<string> calculated = new HashSet<string>();
        HashSet<string> calculating = new HashSet<string>();

        willCalculate.Add(nodeId1);

        List<string> Trace(string destination, string source)
        {
            string current = destination;
            List<string> l = new List<string>();
            l.Add(current);
            int loopCount = 0;
            int maxLoopCount = 1000;
            while (true)
            {
                current = traceDict[current];
                l.Add(current);
                if (current == source)
                    break;
                loopCount++;
                if (loopCount >= maxLoopCount)
                {
                    throw new System.Exception("Max Loop Count! Infinite Loop????");
                }
            }
            l.Reverse();
            return l;
        }

        while (willCalculate.Count > 0)
        {
            calculating.Clear();
            calculating.UnionWith(willCalculate);
            willCalculate.Clear();
            foreach (var nodeId in calculating)
            {
                var linkedNodeIds = GetLinkedNode(nodeId);
                foreach (var linkedNodeId in linkedNodeIds)
                {
                    if (willCalculate.Contains(linkedNodeId) || calculating.Contains(linkedNodeId) || calculated.Contains(linkedNodeId))
                    {
                        continue;
                    }
                    traceDict.Add(linkedNodeId, nodeId);
                    calculated.Add(nodeId);
                    willCalculate.Add(linkedNodeId);
                    if (linkedNodeId == nodeId2)
                    {
                        return Trace(nodeId2, nodeId1);
                    }
                }
            }
        }
        return null;
    }
    #endregion

}
