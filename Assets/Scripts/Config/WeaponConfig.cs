using Combat;
using Combat.Projectile;
using UnityEngine;

namespace Config
{
    public enum DamageType
    {
        Pierce,
        Slash,
        Smash,
        Magic,
        Siege
    }
    
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Config/WeaponConfig")]
    public class WeaponConfig : EquipmentConfig
    {
        [Header("Stats")]
        public WeaponID ID;
        public DamageType DamageType;
        public ProjectileDataSO ProjectileDataConfig;
        public float Damage;
        public float AttackRange;
        public float AttackSpeed;
    }   
}
