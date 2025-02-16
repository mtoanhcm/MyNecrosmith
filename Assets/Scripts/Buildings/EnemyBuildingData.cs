using Config;
using UnityEngine;

namespace Building
{
    public class EnemyBuildingData : BuildingData
    {
        private readonly EnemyBuildingConfig config;

        public CharacterID EnemySpawnID => config.EnemySpawnID;
        public float Cooldown => config.CooldownSpawnTime;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public EnemyBuildingData(BuildingConfig baseConfig) : base(baseConfig)
        {
            config = baseConfig as EnemyBuildingConfig;
            if (config == null)
            {
                Debug.LogError($"Error parse config for {base.baseConfig.BuildingType}");
            }
        }
    }   
}
