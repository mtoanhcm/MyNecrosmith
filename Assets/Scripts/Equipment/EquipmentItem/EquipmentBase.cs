using Character;
using Config;
using UnityEngine;

namespace Equipment
{
    public abstract class EquipmentBase : MonoBehaviour
    {
        public EquipmentData Data { get; protected set; }

        public CharacterBase Owner { get; protected set; }

        public abstract void PerformAction(Transform target);
        public abstract void DeSpawn();
        
        public virtual void Spawn(CharacterBase owner ,EquipmentData data)
        {
            Data = data;   
            Owner = owner;
        }
    }   
}
