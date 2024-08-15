using Config;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public class RewardConstructSpawner : MonoBehaviour
    {
        [SerializeField]
        private BuildingBase rewardBase;

        private MapConfig mapConfig;
        private List<Vector3> rewardBuildingPosLst;

        private void Awake()
        {
            mapConfig = Resources.Load<MapConfig>("MapConfig");
        }

        public async void SpawnRewardConstruct(int areaIndex) {

            rewardBuildingPosLst = new();

            var config = mapConfig.GetAreaByIndex(areaIndex);
            var maxRadius = config.Radius;
            var minRadius = mapConfig.GetMinRadius(areaIndex);
            var totalReward = config.TotalRewards;

            int numTry = 20;
            while (totalReward > 0 || numTry > 0) {
                var randomOffset = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
                Vector3 buildingPos = new (randomOffset.x, randomOffset.y, 0f);

                if (HasValidPosition(buildingPos, config.RewardRadiusGap * config.RewardRadiusGap))
                {
                    Instantiate(rewardBase, buildingPos, Quaternion.identity, transform);
                    rewardBuildingPosLst.Add(buildingPos);
                    totalReward--;
                }
                else {
                    numTry--;
                }

                await UniTask.DelayFrame(1);
            }
        }

        private bool HasValidPosition(Vector3 position, float gapDistance) {

            var compareDistance = gapDistance * gapDistance;
            for (var i = 0; i < rewardBuildingPosLst.Count; i++) { 
                var checkPos = rewardBuildingPosLst[i];
                if ((checkPos - position).sqrMagnitude < compareDistance) {
                    return false;
                }
            }

            return true;
        }
    }
}
