using System.Collections.Generic;
using Equipment;
using InterfaceComp;
using Observer;
using UnityEngine;

namespace Character
{
    public class MinionCharacter : CharacterBase
    {
        public MinionData MinionData => Data as MinionData;

        [SerializeField] private EquipmentController equipmentController;
        [SerializeField] private CharacterAutoActiveBuilding characterAutoActiveBuilding;

        public override void Spawn(CharacterData data)
        {
            base.Spawn(data);
            
            characterAutoActiveBuilding.Init(this);
        }

        public void InitEquipment(EquipmentData[] equipmentData)
        {
            equipmentController.AddEquipment(equipmentData, this);
            MinionData?.SetAttackRange(GetFarthestAttackRangeFromEquipment(equipmentData));
        }

        protected override string GetBrainType()
        {
            return "BehaviourGraph/MinionBrain";
        }

        protected override void OnCharacterDeath()
        {
            base.OnCharacterDeath();
            equipmentController.ReleaseAllEquipment();
            
            EventManager.Instance.TriggerEvent(new EventData.OnMinionDeath(){ Minion = this});
        }

        public override void Attack(Transform target)
        {
            equipmentController.Attack(target);
        }

        private float GetFarthestAttackRangeFromEquipment(EquipmentData[] equipmentData)
        {
            float farthestRange = 0;
            foreach (var data in equipmentData)
            {
                if (data is WeaponData wp && farthestRange < wp.AttackRadius)
                {
                    farthestRange = wp.AttackRadius;
                }
            }
            
            return farthestRange;
        }
    }
}    
