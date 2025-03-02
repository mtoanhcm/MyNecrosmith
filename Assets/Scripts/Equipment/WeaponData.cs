using System.Globalization;
using Config;
using Projectile;
using UnityEngine;

namespace Equipment
{
    public class WeaponData : EquipmentData
    {
        private WeaponConfig weaponConfig => baseConfig as WeaponConfig;
        public override string EffectType => $"Damage Type: {DamageType}";
        public override string EffectValue => $"Damage: {Damage.ToString(CultureInfo.InvariantCulture)}";

        public EquipmentID EquipmentID => baseConfig.EquipmentID;
        public DamageType DamageType => weaponConfig.DamageType;
        public  ProjectileDataSO ProjectileSO => weaponConfig.ProjectileSO;
        public int Damage => weaponConfig.Damage;
        public float AttackRadius => weaponConfig.AttackRange;
        public float AttackSpeed => weaponConfig.AttackSpeed;
        public float Cooldown => weaponConfig.Cooldown;
        
        public WeaponData(EquipmentConfig baseData) : base(baseData)
        {
            
        }
    }   
}
