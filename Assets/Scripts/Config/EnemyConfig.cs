using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
    public class EnemyConfig : CharacterConfig
    {
        public int Damage;
        public DamageType DamageType;
        public ArmorType ArmorType;
    }   
}
