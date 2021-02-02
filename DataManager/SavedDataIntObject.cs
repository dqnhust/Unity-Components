using UnityEngine;

namespace DataManager
{
    [CreateAssetMenu(menuName = "GameData/SavedDataObject/Create SavedDataIntObject", fileName = "SavedDataIntObject", order = 0)]
    public class SavedDataIntObject : AbstractSavedDataObject<int>
    {
        public override string GetValueString()
        {
            return (Value+1).ToString();
        }
    }
}