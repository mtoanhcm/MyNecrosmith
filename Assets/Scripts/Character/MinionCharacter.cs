using System.Collections.Generic;
using Equipment;
using UnityEngine;

namespace Character
{
    public class MinionCharacter : CharacterBase
    {
        public MinionData MinionData => Data as MinionData;

        [SerializeField] private EquipmentController equipmentController;
        
        public void InitEquipment(List<EquipmentData> equipmentData)
        {
            equipmentController.AddEquipment(equipmentData, this);
            MinionData?.SetAttackRange(5);
        }

        protected override string GetBrainType()
        {
            return "BehaviourGraph/MinionBrain";
        }

        protected override void OnCharacterDeath()
        {
            base.OnCharacterDeath();
            equipmentController.ReleaseAllEquipment();
        }

        public override void Attack()
        {
            
        }
        
    }
}    
