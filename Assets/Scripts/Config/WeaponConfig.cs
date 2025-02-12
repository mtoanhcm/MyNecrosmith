using Combat;
using Projectile;
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
        public DamageType DamageType;
        public ProjectileDataSO ProjectileSO;
        public int Damage;
        public float AttackRange;
        public float AttackSpeed;
        public float Cooldown;
    }   
}
