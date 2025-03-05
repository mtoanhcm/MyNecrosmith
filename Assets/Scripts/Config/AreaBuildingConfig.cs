using System;
using Config;
using UnityEngine;

namespace Building
{
    [Serializable]
    public class AreaBuildingConfig
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
}