using Config;
using UnityEngine;

namespace Character
{
    public class EnemyData : CharacterData
    {
        private EnemyConfig enemyConfig => baseConfig as EnemyConfig;

        public int Damage => enemyConfig.Damage;
        public DamageType DamageType => enemyConfig.DamageType;
        public override ArmorType ArmorType => enemyConfig.ArmorType;

        public override float AttackRange => enemyConfig.AttackRange;

        public EnemyData(CharacterConfig config) : base(config)
        {
            
        }
    }   
}
