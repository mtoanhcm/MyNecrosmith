using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Character
{
    public class MinionCharacter : CharacterBase
    {
        public List<EquipmentBase> Equipments => equipments;
        [SerializeField] private List<EquipmentBase> equipments;

        public void InitEquipment(List<EquipmentData> equipmentDatas)
        {
            equipments = new List<EquipmentBase>();
            for (var i = 0; i < equipmentDatas.Count; i++)
            {
                //equipments.Add();
            }
            
            
        }

        protected override void Die()
        {
            
        }
    }
}    
