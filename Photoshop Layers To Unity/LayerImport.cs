#pragma warning disable 0649
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class LayerImport : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Sprite[] layers;
    [SerializeField] private TextAsset positionData;

    private class PositionData
    {
        public int index;

        public string name;

        /// <summary>
        /// Unit: Pixel
        /// </summary>
        public int left;
        /// <summary>
        /// Unit: Pixel
        /// </summary>
        public int top;
        /// <summary>
        /// Unit: Pixel
        /// </summary>
        public int right;
        /// <summary>
        /// Unit: Pixel
        /// </summary>
        public int bottom;

        public Vector2Int Size => new Vector2Int(right - left, top - bottom);

        public PositionData(int index, string name, int left, int top, int right, int bottom)
        {
            this.index = index;
            this.name = name;
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }

    public void OptimizeResource()
    {
        Dictionary<string, string> dictSameLayer = new Dictionary<string, string>();
        dictSameLayer.Clear();
        List<Sprite> spriteNotUse = new List<Sprite>();
        List<Sprite> listLayer = new List<Sprite>();
        var n = layers.Length;
        Sprite sp1 = null;
        Sprite sp2 = null;
        for (int i = 0; i < n; i++)
        {
            sp1 = layers[i];
            if (spriteNotUse.Contains(sp1))
            {
                //Debug.Log("Sp1 Not Use!");
                continue;
            }
            for (int j = i + 1; j < n; j++)
            {
                sp2 = layers[j];
                if (sp2.texture.GetInstanceID().Equals(sp1.texture.GetInstanceID()))
                    continue;
                bool isSame = sp2.texture.imageContentsHash.Equals(sp1.texture.imageContentsHash);
                if (isSame)
                {
                    dictSameLayer[sp2.name] = sp1.name;
                    spriteNotUse.Add(sp2);
                }
            }
            listLayer.Add(sp1);
        }
        Debug.Log("Old:" + layers.Length + "=> New:" + listLayer.Count + " NotUser:" + spriteNotUse.Count);
        layers = listLayer.ToArray();
        string path = string.Empty;
        foreach (var sprite in spriteNotUse)
        {
            path = AssetDatabase.GetAssetPath(sprite);
            var noNamePath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            fileName = "NotUse_" + fileName;
            AssetDatabase.RenameAsset(path, fileName);
        }
        Debug.Log("Have " + spriteNotUse.Count + " sprites not use! Renamed It!");

        //

        List<PositionData> data = DecodePositionData(positionData);
        bool changed = false;
        bool contain = false;
        string oriName = string.Empty;
        foreach (var item in data)
        {
            contain = dictSameLayer.TryGetValue(item.name, out oriName);
            if (contain)
            {
                item.name = oriName;
                changed = true;
            }
        }
        if (changed)
        {
            var newData = EncodePositionData(data);
            var textAssetpath = AssetDatabase.GetAssetPath(positionData);
            int l = "Assets/".Length;
            textAssetpath = textAssetpath.Substring(l, textAssetpath.Length - l);
            textAssetpath = Path.Combine(Application.dataPath, textAssetpath);
            File.WriteAllText(textAssetpath, newData);
            Debug.Log("Changed File: " + textAssetpath);
        }
    }

    public void Convert()
    {
        var n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        if (layers.Length == 0 || positionData == null)
        {
            Debug.LogError("Fill Data and Layers to Convert!");
            return;
        }
        List<PositionData> listData = DecodePositionData(positionData);
        float pixelPerUnit = 1;
        int itemCount = listData.Count;
        Debug.Log("Image In Scene:" + itemCount + "/ Total Sprites:" + layers.Length);
        foreach (var item in listData)
        {
            var sprite = FindSprite(item.name);
            if (sprite == null)
                continue;
            Undo.RecordObject(gameObject, "Generate New Child");
            pixelPerUnit = item.Size.x / sprite.bounds.size.x;
            var g = new GameObject(item.name, typeof(SpriteRenderer));
            Vector3 pos = new Vector3(item.left, -item.top, 0) / pixelPerUnit;
            var worldSize = sprite.bounds.size;
            pos.x += worldSize.x / 2f;
            pos.y -= worldSize.y / 2f;
            g.transform.SetParent(transform);
            PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
            Undo.RecordObject(g.transform, "Set Position");
            g.transform.localPosition = pos;
            PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
            Undo.RecordObject(g, "Set Sprite");
            var spr = g.GetComponent<SpriteRenderer>();
            spr.sprite = sprite;
            Undo.RecordObject(g, "Set Sprite Order");
            spr.sortingOrder = itemCount - item.index;
            PrefabUtility.RecordPrefabInstancePropertyModifications(spr);
            //EditorUtility.SetDirty(g);
        }

        //EditorSceneManager.MarkAllScenesDirty();
        EditorUtility.SetDirty(this);
    }

    private Sprite FindSprite(string name)
    {
        foreach (var layer in layers)
        {
            if (layer.name.Equals(name))
            {
                return layer;
            }
        }
        return null;
    }


    private List<PositionData> DecodePositionData(TextAsset asset)
    {
        List<PositionData> data = new List<PositionData>();
        var lines = asset.text.Split('|');
        string layerName = "";
        int left, top, right, bottom;
        int layerIndex = 0;
        foreach (var line in lines)
        {
            string[] arr = line.Split(',');
            if (arr.Length != 6)
                continue;
            //Debug.Log("====");
            //foreach (var s in arr)
            //{
            //    Debug.Log(s);
            //}
            //Debug.Log("====");
            layerIndex = int.Parse(arr[0]);
            layerName = arr[1];
            left = int.Parse(arr[2]);
            top = int.Parse(arr[3]);
            right = int.Parse(arr[4]);
            bottom = int.Parse(arr[5]);
            data.Add(new PositionData(layerIndex, layerName, left, top, right, bottom));
        }
        return data;
    }

    private string EncodePositionData(List<PositionData> data)
    {
        string returnString = string.Empty;
        string currentS = string.Empty;
        int i = 0;
        foreach (var item in data)
        {
            //coords += i + "," + layerRef.name + "," + x + "," + y + "," + w + "," + h + "|";
            currentS = i + "," + item.name + "," + item.left + "," + item.top + "," + item.right + "," + item.bottom + "|";
            returnString += currentS;
            i++;
        }
        return returnString;
    }
#endif
}