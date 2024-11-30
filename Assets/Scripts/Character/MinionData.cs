using Config;
using UnityEngine;

namespace Character
{
    public class MinionData : CharacterData
    {
        private MinionConfig minionConfig => baseConfig as MinionConfig;
        
        public CharacterClass Class => minionConfig.Class;
        public CharacterRace Race => minionConfig.Race;
        public Vector2Int InventorySize => minionConfig.InventorySize;
        public override ArmorType ArmorType => armorType;

        private ArmorType armorType;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public MinionData(CharacterConfig config) : base(config)
        {
        }

        public void SetArmorType(ArmorType armorType)
        {
            this.armorType = armorType;
        }
    }   
}
