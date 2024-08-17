using Config;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Building {
    public class RewardConstructSpawner
    {
        [SerializeField]
        private BuildingBase rewardBase;

        private MapConfig mapConfig;
        private Dictionary<int, Vector3> rewardBuildingDic;

        public RewardConstructSpawner()
        {
            mapConfig = Resources.Load<MapConfig>("MapConfig");
        }

        public async void SpawnRewardConstruct(int areaIndex, UnityAction<List<Vector3>> onCreateStructureSuccess) {

            rewardBuildingDic = new();

            var config = mapConfig.GetAreaByIndex(areaIndex);
            var maxRadius = config.Radius;
            var minRadius = mapConfig.GetMinRadius(areaIndex);
            var totalReward = config.TotalRewards;

            int numTry = 20;
            int index = 0;
            while (totalReward > 0 && numTry > 0) {
                var randomOffset = Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
                Vector3 buildingPos = new (randomOffset.x, randomOffset.y, 0f);

                if (HasValidPosition(buildingPos, config.RewardRadiusGap))
                {
                    Object.Instantiate(rewardBase, buildingPos, Quaternion.identity);

                    rewardBuildingDic.Add(index, buildingPos);

                    index++;
                    totalReward--;
                }
                else {
                    numTry--;
                }

                await UniTask.DelayFrame(1);
            }

            onCreateStructureSuccess?.Invoke(rewardBuildingDic.Values.ToList());
        }

        public void DespawnRewardConstruct(int id, UnityAction<Vector3> despawnRewardConstructSuccess) {
            if (rewardBuildingDic.ContainsKey(id) == false) {
                return;
            }

            despawnRewardConstructSuccess?.Invoke(rewardBuildingDic[id]);
            rewardBuildingDic.Remove(id);
        }

        private bool HasValidPosition(Vector3 position, float gapDistance) {

            var compareDistance = gapDistance * gapDistance;
            foreach (var building in rewardBuildingDic) {
                if ((building.Value - position).sqrMagnitude < compareDistance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
