using UnityEngine;

namespace Config
{
    public enum ArmorType
    {
        Bone,
        Flesh,
        Armor,
        Building
    }
    
    [CreateAssetMenu(fileName = "ArmorConfig", menuName = "baseConfig/Equipment/ArmorConfig")]
    public class ArmorConfig : EquipmentConfig
    {
        public ArmorType Type;
        public int HP;
    }   
}
