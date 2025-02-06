using Config;
using Projectile;
using UnityEngine;

namespace Character
{
    public class EnemyData : CharacterData
    {
        private EnemyConfig enemyConfig => baseConfig as EnemyConfig;

        public int Damage => enemyConfig.Damage;
        public float Cooldown => enemyConfig.Cooldown;
        public ProjectileDataSO ProjectileSO => enemyConfig.ProjectileSO;
        public DamageType DamageType => enemyConfig.DamageType;
        public override ArmorType ArmorType => enemyConfig.ArmorType;

        public override float AttackRange => enemyConfig.AttackRange;
        public override float AttackSpeed => enemyConfig.AttackSpeed;

        public EnemyData(CharacterConfig config) : base(config)
        {
            
        }
    }   
}
