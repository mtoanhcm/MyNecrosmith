using Config;
using UnityEngine;

namespace Building
{
    public class MinionBuildingData : BuildingData
    {
        private readonly MinionBuildingConfig config;
        
        
        public MinionBuildingData(BuildingConfig baseConfig, int areaIndex, int level) : base(baseConfig, areaIndex, level)
        {
            config = baseConfig as MinionBuildingConfig;
            if (config == null)
            {
                Debug.LogError($"Error parse config for {base.baseConfig.ID}");
            }
        }
    }   
}
