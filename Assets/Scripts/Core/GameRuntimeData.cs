using System;
using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameRuntimeData", menuName = "Data/GameRuntimeData")]
    public class GameRuntimeData : ScriptableObject
    {
        public List<EquipmentData> EquipmentStorage => equipmentStorage;

        [SerializeField]
        private List<EquipmentData> equipmentStorage;

        public void Reset()
        {
            equipmentStorage = new List<EquipmentData>();
        }
        
        public bool AddEquipmentToStorage(EquipmentData equipment)
        {
            if (equipmentStorage == null)
            {
                equipmentStorage = new List<EquipmentData>();
            }
            
            equipmentStorage.Add(equipment);
            return true;
        }

        public void RemoveEquipmentFromStorage(EquipmentID id)
        {
            var equipmentIndex = equipmentStorage.FindIndex(item =>  item.EquipmentID == id);
            if (equipmentIndex < 0)
            {
                return;
            }
            
            equipmentStorage.RemoveAt(equipmentIndex);
        }
    }   
}
