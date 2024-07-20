using System;
using UnityEngine;

namespace Config {
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Config/MapConfig", order = 1)]
    public class MapConfig : ScriptableObject
    {
        [Serializable]
        public struct AreaData {
            [Tooltip("Radius of Area")]
            public int Radius;
            [Tooltip("Total rewards in Area")]
            public int TotalRewards;
            [Tooltip("Quality of rewards in Area")]
            public int RewardQuality;
            [Tooltip("Total enemies in Area")]
            public int TotalEnemies;
            [Tooltip("Total bosses in Area")]
            public int TotalBosses;
        }

        [Tooltip("First Area data config")]
        public AreaData FirstArea;
        [Tooltip("Middle Area data config")]
        public AreaData MidArea;
        [Tooltip("Last area data config")]
        public AreaData LastArea;
    }
}
