using Config;
using UnityEngine;

namespace Character
{
    public class MinionData : CharacterData
    {
        private MinionConfig minionConfig => baseConfig as MinionConfig;
        public CharacterRace Race => minionConfig.Race;
        public Vector2Int InventorySize => minionConfig.InventorySize;
        public override ArmorType ArmorType => armorType;
        public override float AttackRange => attackRange;

        private ArmorType armorType;
        private float attackRange;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public MinionData(CharacterConfig config) : base(config)
        {
            
        }

        public void SetArmorType(ArmorType type)
        {
            this.armorType = type;
        }

        public void SetAttackRange(float range)
        {
            Debug.Log($"Set attack range {range}");
            attackRange = range;
        }
    }   
}
