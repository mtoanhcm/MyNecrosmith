using Config;
using UnityEngine;

namespace Equipment
{
    public abstract class EquipmentBase : MonoBehaviour
    {
        protected EquipmentStats Stats;

        public abstract void Action();
        public abstract void DeSpawn();
        
        public virtual void Spawn(EquipmentConfig equipmentConfig)
        {
            Stats = new EquipmentStats(equipmentConfig);   
        }
    }   
}
