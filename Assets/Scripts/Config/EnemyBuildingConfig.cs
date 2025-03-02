using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EnemyBuildingConfig", menuName = "baseConfig/Building/EnemyBuildingConfig")]
    public class EnemyBuildingConfig : BuildingConfig
    {
        public EnemyConfig EnemySpawnConfig;
        public float CooldownSpawnTime;
    }
}
