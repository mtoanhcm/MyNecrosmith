using Character;
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

        [Header("Projectile")]
        [SerializeField] protected ProjectileID projectileID;
        [SerializeField] protected ProjectileSpawner projectileSpawner;
        
        protected ProjectileDataSO projectileData;
        
        public abstract void PerformAction(Transform target);
        public abstract void DeSpawn();
        
        public virtual void Init(CharacterBase owner ,EquipmentData data, Vector3 spawnPosition)
        {
            Data = data;   
            Owner = owner;
            
            transform.position = spawnPosition;

            projectileData = Resources.Load<ProjectileDataSO>($"Projectiles/{projectileID}");
        }

        protected virtual void OnKillTargetSuccess()
        {
            
        }
    }   
}
