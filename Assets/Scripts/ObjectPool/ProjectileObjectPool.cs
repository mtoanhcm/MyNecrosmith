using Observer;
using UnityEngine;
using Pool;
using Projectile;

namespace Spawner
{
    public class ProjectileObjectPool : MonoBehaviour
    {
        private ObjectPool<ProjectileBase> pool;

        private void Awake()
        {
            pool = new ObjectPool<ProjectileBase>(transform);
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnSpawnProjectile>(OnSpawnProjectile);
            EventManager.Instance.StartListening<EventData.OnDespawnProjectile>(OnDespawnProjectile);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OnSpawnProjectile>(OnSpawnProjectile);
            EventManager.Instance?.StopListening<EventData.OnDespawnProjectile>(OnDespawnProjectile);
            
            pool.Dispose();
        }

        private async void OnSpawnProjectile(EventData.OnSpawnProjectile data)
        {
            var projectile = await pool.Get($"Projectiles/{data.ProjectileID}.prefab");
            if (projectile == null)
            {
                Debug.LogWarning($"Cannot spawn equipment {data.ProjectileID}");
                return;
            }
            
            data.OnSpawnSuccess?.Invoke(projectile);
        }
        
        private void OnDespawnProjectile(EventData.OnDespawnProjectile data)
        {
            pool.Return($"Projectiles/{data.Projectile.Data.ID}.prefab", data.Projectile);
            data.Projectile.ResetData();
        }
    }   
}
