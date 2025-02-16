using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EnemyBuildingConfig", menuName = "baseConfig/Building/EnemyConfig")]
    public class EnemyBuildingConfig : BuildingConfig
    {
        public CharacterID EnemySpawnID;
        public float CooldownSpawnTime;
    }
}
