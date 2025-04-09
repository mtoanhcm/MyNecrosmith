using Combat;
using Config;
using UnityEngine;

namespace Projectile
{
    public class ProjectileData
    {
        public ProjectileID ID => Config.ProjectileID;
        public ProjectileDataSO Config { get; private set; }
        public HitData HitData => hitData;
        public int Damage => hitData.Amount;
        public float MoveSpeed => hitData.AttackSpeed;
        public float AttackRange => hitData.AttackRange;
        public Vector3 SpawnPosition => hitData.SpawnPos;
        public Vector3 Direction => hitData.Direction;
        public GameObject Attacker => hitData.Attacker;

        private readonly HitData hitData;

        public ProjectileData(HitData data)
        {
            Config = data.ProjectileConfig;
            hitData = data;
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
