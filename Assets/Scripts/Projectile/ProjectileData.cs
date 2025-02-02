using Combat;
using Config;
using UnityEngine;

namespace Projectile
{
    public class ProjectileData
    {
        public ProjectileID ID => Config.ProjectileID;
        public ProjectileDataSO Config { get; private set; }
        public float Damage => attackData.Damage;
        public float MoveSpeed => attackData.AttackSpeed;
        public float AttackRange => attackData.AttackRange;
        public Vector3 SpawnPosition => attackData.SpawnPos;
        public Vector3 Direction => attackData.Direction;

        private readonly AttackData attackData;

        public ProjectileData(AttackData data)
        {
            Config = data.ProjectileConfig;
            attackData = data;
        }

        public void Fire(ProjectileBase projectile)
        {
            Config.ProjectileMovement.StartMovement(projectile, ApplyDamage);
        }

        private void ApplyDamage(ProjectileBase projectile)
        {
            Config.DamageApplication.DetectAndApplyDamage(projectile);
        }
    }
}
