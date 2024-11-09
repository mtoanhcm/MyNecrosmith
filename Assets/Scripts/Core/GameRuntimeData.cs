using System.Collections.Generic;
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

        public void AddEquipmentToStorage(EquipmentData equipment)
        {
            if (equipmentStorage == null)
            {
                equipmentStorage = new List<EquipmentData>();
            }
            
            equipmentStorage.Add(equipment);
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
