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
        public WeaponID ID;
        public DamageType DamageType;
        public float Damage;
        public float AttackRange;
        public float AttackSpeed;
    }   
}
