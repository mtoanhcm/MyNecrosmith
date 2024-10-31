using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Character
{
    public class MinionCharacter : CharacterBase
    {
        public List<EquipmentBase> Equipments => equipments;
        private List<EquipmentBase> equipments;

        public void InitEquipment(List<EquipmentData> equipmentDatas)
        {
            equipments = new List<EquipmentBase>();
        }

        protected override void Die()
        {
            
        }
    }
}    
