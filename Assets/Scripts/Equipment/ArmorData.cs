using System.Globalization;
using Config;
using UnityEngine;

namespace Equipment
{
    public class ArmorData : EquipmentData
    {
        private ArmorConfig armorConfig => baseConfig as ArmorConfig;
        public override string EffectType => $"Armor Type: {ArmorType}";
        public override string EffectValue => $"MaxHPStat: {HP.ToString(CultureInfo.InvariantCulture)}";
        public ArmorType ArmorType => armorConfig.Type;
        public int HP => armorConfig.HP;
        
        public ArmorData(EquipmentConfig config) : base(config)
        {
            
        }
    }   
}
