using System.Collections.Generic;
using Config;
using Equipment;
using GameUtility;
using Observer;
using UnityEngine;

namespace Character
{
    public class MinionCharacter : CharacterBase
    {
        public List<EquipmentBase> Equipments => equipments;
        
        private List<EquipmentBase> equipments;

        [SerializeField] private Transform equipmentContainer;

        public void InitEquipment(List<EquipmentData> equipmentDatas)
        {
            var equipmentPositions = equipmentContainer.position.GetEquipmentPositionAroundCharacter(equipmentDatas.Count);
            equipments = new List<EquipmentBase>();
            for (var i = 0; i < equipmentDatas.Count; i++)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnSpawnEquipment()
                {
                    Equipment = equipmentDatas[i],
                    SpawnPosition = equipmentPositions[i],
                    Owner = this,
                    OnSpawnEqupimentSuccessHandle = OnSpawnEquipmentSuccess
                });
            }

            return;
            
            void OnSpawnEquipmentSuccess(EquipmentBase equipment)
            {
                equipments.Add(equipment);
                equipment.transform.SetParent(equipmentContainer);
            }
        }

        protected override async void SetupModel(CharacterID id)
        {
            _ = await AddressableUtility.InstantiateAsync($"Model/Minion/{id}.prefab", transform);
        }
    }
}    
