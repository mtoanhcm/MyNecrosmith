using Projectile;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "baseConfig/Character/EnemyConfig")]
    public class EnemyConfig : CharacterConfig
    {
        public int Damage;
        public float AttackRange;
        public float Cooldown;
        public DamageType DamageType;
        public ArmorType ArmorType;
        public ProjectileDataSO ProjectileSO;
    }   
}
