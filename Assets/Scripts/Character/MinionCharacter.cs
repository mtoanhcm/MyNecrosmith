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
        public MinionData MinionData => Data as MinionData;
        
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
            
            MinionData?.SetAttackRange(5);
            
            return;
            
            void OnSpawnEquipmentSuccess(EquipmentBase equipment)
            {
                equipments.Add(equipment);
                equipment.transform.SetParent(equipmentContainer);
            }
        }

        protected override string GetBrainType()
        {
            return "BehaviourGraph/MinionBrain";
        }

        public override void Attack()
        {
            
        }
    }
}    
