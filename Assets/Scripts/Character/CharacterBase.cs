using System.Collections;
using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] protected Transform equipmentContainer;
        
        protected CharacterStats characterStats;
        protected List<EquipmentBase> equipments;
        
        public abstract void Attack();
        public abstract void AbilityActive();
        public abstract void Move();
        public abstract void Death();

        public virtual void Spawn(CharacterConfig data, List<EquipmentConfig> equipmentsInit)
        {
            characterStats = new CharacterStats(data);
            
            equipments = new List<EquipmentBase>();
            for (var i = 0; i < equipmentsInit.Count; i++)
            {
                var equipmentInit = equipmentsInit[i];
                var equipment = Instantiate(equipmentInit.EquipmentPrefab, equipmentContainer);
                equipment.Spawn(equipmentInit);
                
                equipments.Add(equipment);
            }
        }
    }   
}
