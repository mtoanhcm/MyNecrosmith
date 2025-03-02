using UnityEngine;

namespace Building
{
    [CreateAssetMenu(fileName = "BuildingSpawnConfig", menuName = "baseConfig/Building/BuildingSpawnConfig")]
    public class BuildingSpawnConfig : ScriptableObject
    {
        [Tooltip("Configuration for each area")]
        public AreaBuildingConfig[] AreaConfigs;

        /// <summary>
        /// Gets the area configuration by index
        /// </summary>
        /// <param name="areaIndex">Area index to retrieve</param>
        /// <returns>Area building configuration or null if not found</returns>
        public AreaBuildingConfig GetAreaConfig(int areaIndex)
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