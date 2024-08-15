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
            [Tooltip("Distance between 2 reward building")]
            public float RewardRadiusGap;
            [Tooltip("Distance between 2 enemy base")]
            public float EnemyBaseRadiusGap;
        }

        [SerializeField]
        private AreaData[] Areas;

        public AreaData GetAreaByIndex(int index) {
            return Areas[Mathf.Clamp(index, 0, Areas.Length - 1)];
        }

        public float GetMinRadius(int index) {
            index = Mathf.Clamp(index, 0, Areas.Length - 1);
            if (index == 0) {
                return 0;
            }

            return Areas[index].Radius - Areas[index - 1].Radius;
        }
    }
}
