using Building;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config {

    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Config/BuildingConfig", order = 1)]
    public class BuildingConfig : ScriptableObject
    {
        [Serializable]
        public struct BuildingDataConfig {
            public BuildingData BaseData;
            public BuildingBase BuildingObj;
        }

        [SerializeField]
        private BuildingDataConfig[] rewardBuilding;
        [SerializeField]
        private BuildingDataConfig[] enemyBuilding;
        [SerializeField]
        private BuildingDataConfig[] mainBuilding;

        public BuildingDataConfig GetRandomRewardBuilding() {
            return rewardBuilding[UnityEngine.Random.Range(0, rewardBuilding.Length)];
        }

        public BuildingDataConfig GetRandomEnemyBuilding()
        {
            return enemyBuilding[UnityEngine.Random.Range(0, enemyBuilding.Length)];
        }

        public bool TryGetMainBuilding(BuildingType type, out BuildingDataConfig data) {
            data = default;

            for (int i = 0; i < mainBuilding.Length; i++) {
                if (mainBuilding[i].BaseData.Type == type) { 
                    data = mainBuilding[i];
                    return true;
                }
            }

            return false;
        }
    }
}
