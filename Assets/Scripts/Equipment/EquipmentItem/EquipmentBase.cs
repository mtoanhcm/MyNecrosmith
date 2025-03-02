using Character;
using Combat;
using Config;
using Projectile;
using Spawner;
using UnityEngine;

namespace Equipment
{
    public abstract class EquipmentBase : MonoBehaviour
    {
        public EquipmentData Data { get; protected set; }

        public CharacterBase Owner { get; protected set; }

        public abstract void PerformAction(Transform target);

        public virtual void DeSpawn()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Init(CharacterBase owner ,EquipmentData data, Vector3 spawnPosition)
        {
            Data = data;   
            Owner = owner;
            
            transform.position = spawnPosition;
        }
        
        protected virtual void OnKillTargetSuccess()
        {
            
        }
    }   
}
