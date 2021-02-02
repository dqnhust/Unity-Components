using UnityEngine;

namespace DataManager
{
    public class DataManager : MonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                DataStorage.SaveData(true);
            }
        }
    }
}