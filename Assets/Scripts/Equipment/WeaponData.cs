using System.Globalization;
using Config;
using UnityEngine;

namespace Equipment
{
    public class WeaponData : EquipmentData
    {
        private WeaponConfig weaponConfig => baseConfig as WeaponConfig;

        public override string ID => WeaponID.ToString();
        public override string EffectType => $"Damage Type: {DamageType}";
        public override string EffectValue => $"Damage: {Damage.ToString(CultureInfo.InvariantCulture)}";

        public WeaponID WeaponID => weaponConfig.ID;
        public DamageType DamageType => weaponConfig.DamageType;
        public float Damage => weaponConfig.Damage;
        public float AttackRadius => weaponConfig.AttackRange;
        public float AttackSpeed => weaponConfig.AttackSpeed;
        
        public WeaponData(EquipmentConfig baseData) : base(baseData)
        {
            
        }
    }   
}
