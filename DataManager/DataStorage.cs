using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public static class DataStorage
    {
        private static Data currentData;

        private static string DataPath
        {
            get { return Path.Combine(Application.persistentDataPath, "GameData.data"); }
        }

        public static void SaveData(bool forceSave = false)
        {
            if (!dataChanged && !forceSave)
            {
                Debug.Log("Nothing Data Change! Not Save!");
                return;
            }

            if (currentData == null)
            {
                Debug.Log("Data is Null, Don't Need Save");
                return;
            }

            var fs = File.Open(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, currentData);
            fs.Close();
            dataChanged = false;
            Debug.Log("Saved Data:" + DataPath);
        }

        public static void LoadData()
        {
            if (currentData != null)
            {
                Debug.LogWarning("Data Loaded Don't need load more time!");
                return;
            }

            if (!File.Exists(DataPath))
            {
                currentData = new Data();
                return;
            }

            try
            {
                var fs = File.Open(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                BinaryFormatter bf = new BinaryFormatter();
                currentData = (Data) bf.Deserialize(fs);
                fs.Close();
                Debug.Log("Loaded Data:" + DataPath);
            }
            catch
            {
                currentData = new Data();
                Debug.Log("Loaded Default Data!");
            }
        }

        public static bool dataChanged = false;

        public static void SetData(string key, object data)
        {
            if (currentData == null)
            {
                LoadData();
                //throw new System.Exception("Need Load Data Before SetData");
            }

            currentData.dict[key] = data;
            dataChanged = true;
        }

        public static T GetData<T>(string key, T defaultValue)
        {
            if (currentData == null)
            {
                LoadData();
                //throw new System.Exception("Need Load Data Before GetData");
            }

            object obj = null;
            bool got = currentData.dict.TryGetValue(key, out obj);
            if (got)
            {
                return (T) obj;
            }
            else
            {
                SetData(key, defaultValue);
                return defaultValue;
            }
        }

        [Serializable]
        private class Data
        {
            public Dictionary<string, object> dict;

            public Data()
            {
                dict = new Dictionary<string, object>();
            }
        }

#if UNITY_EDITOR
        [MenuItem("DataStoreAge/DeleteAll")]
#endif
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            File.Delete(DataPath);
            currentData = null;
            Debug.LogWarning("Deleted Data:" + DataPath);
            LoadData();
        }
    }
}