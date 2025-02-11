using Combat;
using Observer;
using Projectile;
using UnityEngine;

namespace Spawner
{
    public class ProjectileSpawner : MonoBehaviour
    {
        public void SpawnProjectile(AttackData data)
        {
            var spawnData = new EventData.OnSpawnProjectile()
            {
                ProjectileID = data.ProjectileConfig.ProjectileID.ToString(),
                OnSpawnSuccess = projectile => OnSpawnedProjectile(projectile, data)
            };
            
            EventManager.Instance.TriggerEvent(spawnData);
        }

        private void OnSpawnedProjectile(ProjectileBase projectile, AttackData attackData)
        {
            projectile.Initialize(attackData);
        }
    }   
}
