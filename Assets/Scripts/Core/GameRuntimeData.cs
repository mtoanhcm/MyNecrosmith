using System.Collections.Generic;
using Equipment;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameRuntimeData", menuName = "Data/GameRuntimeData")]
    public class GameRuntimeData : ScriptableObject
    {
        public List<EquipmentData> EquipmentStorage => equipmentStorage;
        public int StorageMax => storageMax;

        private List<EquipmentData> equipmentStorage;
        private int storageMax;
        
        public bool AddEquipmentToStorage(EquipmentData equipment)
        {
            if (equipmentStorage == null)
            {
                equipmentStorage = new List<EquipmentData>();
            }

            if (equipmentStorage.Count >= storageMax)
            {
                return false;
            }
            
            equipmentStorage.Add(equipment);
            return true;
        }

        public void RemoveEquipmentFromStorage(EquipmentData equipment)
        {
            if (!equipmentStorage.Contains(equipment))
            {
                return;
            }
            
            equipmentStorage.Remove(equipment);
        }
    }   
}
