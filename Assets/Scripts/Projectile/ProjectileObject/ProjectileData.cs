using Combat;
using Config;
using UnityEngine;

namespace Projectile
{
    public class ProjectileData
    {
        public ProjectileID ID => Config.ProjectileID;
        public ProjectileDataSO Config { get; private set; }
        public int Damage => attackData.Damage;
        public float MoveSpeed => attackData.AttackSpeed;
        public float AttackRange => attackData.AttackRange;
        public Vector3 SpawnPosition => attackData.SpawnPos;
        public Vector3 Direction => attackData.Direction;
        public GameObject Attacker => attackData.Attacker;

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

        private bool ApplyDamage(ProjectileBase projectile)
        {
            return Config.DamageApplication.DetectAndApplyDamage(projectile);
        }
    }
}
