using Character;
using Combat;
using Config;
using Equipment;
using UnityEngine;

namespace Projectile
{
    public class ProjectileData
    {
        public ProjectileID ID => Config.ProjectileID;
        public ProjectileDataSO Config { get; private set; }
        public float Damage { get; private set; }
        public float MoveSpeed { get; private set; }

        public ProjectileData(AttackData data)
        {
            Config = data.ProjectileConfig;

            Damage = data.Damage;
            MoveSpeed = data.AttackSpeed;
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
