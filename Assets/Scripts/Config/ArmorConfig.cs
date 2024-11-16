using UnityEngine;

namespace Config
{
    public enum ArmorID
    {
        BareBody,
        LeatherBody,
        LightArmor,
        ChainArmor,
        HeavyArmor,
        SilverArmor,
        GoldenArmor,
    }

    public enum ArmorType
    {
        Bone,
        Flesh,
        Armor,
        Building
    }
    
    [CreateAssetMenu(fileName = "ArmorConfig", menuName = "Config/ArmorConfig")]
    public class ArmorConfig : ScriptableObject
    {
        public ArmorID ID;
        public string Name;
        public int HP;
        public ArmorType Type;
        public int LoadPoint;
    }   
}
