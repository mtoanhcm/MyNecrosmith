using System;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BuildingSpawnConfig", menuName = "baseConfig/Building/BuildingSpawnConfig")]
    public class BuildingSpawnConfig : ScriptableObject
    {
        [Serializable]
        public class AreaBuildingData
        {
            [Tooltip("Index of this area")]
            public int AreaIndex;

            [Tooltip("Minimum spawn radius from map center")]
            public float SpawnRangeMin;

            [Tooltip("Maximum spawn radius from map center")]
            public float SpawnRangeMax;

            [Tooltip("Building level for this area")]
            public int BuildingLevel;

            [Tooltip("Number of buildings to spawn in this area")]
            public int BuildingAmount;

            [Tooltip("Minimum distance between buildings in the same area")]
            public float BuildingDistance;

            [Tooltip("Available enemy building configurations for this area")]
            public EnemyBuildingConfig[] EnemyBuildingConfigs;
        }

        [Serializable]
        public class AreaBuildingLevel {
            public int Level;
            public float Rate;

        }


        [Tooltip("Configuration for each area")]
        public AreaBuildingData[] AreaConfigs;

        /// <summary>
        /// Gets the area configuration by index
        /// </summary>
        /// <param name="areaIndex">Area index to retrieve</param>
        /// <returns>Area building configuration or null if not found</returns>
        public AreaBuildingData GetAreaConfig(int areaIndex)
        {
            if (AreaConfigs == null) return null;
            
            foreach (var areaConfig in AreaConfigs)
            {
                if (areaConfig.AreaIndex == areaIndex)
                {
                    return areaConfig;
                }
            }

            Debug.LogWarning($"Area config with index {areaIndex} not found");
            return null;
        }
    }
}