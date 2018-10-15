using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Awake()
    {
        DataStoreAge.LoadData();
    }

    private void OnEnable()
    {
        StartCoroutine(IECheckSaveData());
    }

    IEnumerator IECheckSaveData()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (DataStoreAge.dataChanged)
            {
                DataStoreAge.SaveData();
            }
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (DataStoreAge.dataChanged)
            {
                DataStoreAge.SaveData();
            }
        }
    }
}
