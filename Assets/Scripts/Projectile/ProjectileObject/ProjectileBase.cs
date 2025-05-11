using Combat;
using Observer;
using UnityEngine;

namespace Projectile
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        public ProjectileData Data => data;
        
        private ProjectileData data;

        public void Initialize(HitData hitData)
        {
            data = new ProjectileData(hitData);
            
            var rotation = Quaternion.LookRotation(hitData.Direction, Vector3.up);
            rotation *= Quaternion.Euler(90f, 0f, 0f);
            
            transform.SetPositionAndRotation(hitData.SpawnPos, rotation);
            data.Fire(this);
        }

        public void ResetData()
        {
            data = null;
        }

        public virtual void Despawn()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnDespawnProjectile(){ ID = data.ID, Projectile = this});
        }
        
        // Method to get damage area info for DamageApplicationSO
        public abstract (Vector3 center, Vector3 size, Quaternion rotation) GetDamageArea();
    }   
}

