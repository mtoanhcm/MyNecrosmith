using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Config;
using Observer;
using Cysharp.Threading.Tasks;
using System.Linq;
using Tile;
using System;
using UnityEngine.Events;

namespace Building {
    public class ConstructSpawner
    {
        private readonly MapConfig mapConfig;
        private readonly BuildingConfig buildingConfig;

        private Dictionary<int, BuildingBase> rewardBuildingDic = new();
        private Dictionary<int, BuildingBase> enemyBuildingDic = new();

        public ConstructSpawner()
        {
            mapConfig = Resources.Load<MapConfig>("MapConfig");
            buildingConfig = Resources.Load<BuildingConfig>("BuildingConfig");
        }

        public void SpawnMainConstruct(BuildingType buildingType) {
            if (buildingConfig.TryGetMainBuilding(buildingType, out var data) == false) {
                return;
            }

            var building = UnityEngine.Object.Instantiate(data.BuildingObj, Vector3.zero, Quaternion.identity);
            building.Init(0, Vector3.zero, data.BaseData);
        }

        public void SpawnConstruct(int areaIndex)
        {
            var config = mapConfig.GetAreaByIndex(areaIndex);

            SpawnBuildings(config, rewardBuildingDic, config.TotalRewards,
                config.RewardRadiusGap, buildingConfig.GetRandomRewardBuilding);

            SpawnBuildings(config, enemyBuildingDic, config.TotalEnemies, config.EnemyBaseRadiusGap,
                buildingConfig.GetRandomEnemyBuilding, rewardBuildingDic, 3f);
        }

        private async void SpawnBuildings(MapConfig.AreaData config, Dictionary<int, BuildingBase> buildingDic,
            int totalBuildings, float gapDistance, Func<BuildingConfig.BuildingDataConfig> getBuildingFunc,
            Dictionary<int, BuildingBase> otherBuildingDic = null, float? otherGapDistance = null)
        {
            var maxRadius = config.Radius;
            var minRadius = mapConfig.GetMinRadius(config.Index);

            int numTry = 20;
            int index = 0;
            while (totalBuildings > 0 && numTry > 0)
            {
                var randomOffset = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(minRadius, maxRadius);
                Vector3 buildingPos = new(randomOffset.x, randomOffset.y, 0f);

                if (IsValidPosition(buildingPos, gapDistance, buildingDic) &&
                    (otherBuildingDic == null || IsValidPosition(buildingPos, otherGapDistance.Value, otherBuildingDic)))
                {
                    var buildingConfig = getBuildingFunc();
                    var building = UnityEngine.Object.Instantiate(buildingConfig.BuildingObj, buildingPos, Quaternion.identity);

                    building.Init(index, buildingPos, buildingConfig.BaseData);

                    buildingDic.Add(index, building);
                    index++;
                    totalBuildings--;
                }
                else
                {
                    numTry--;
                }

                await UniTask.DelayFrame(1);
            }

            EventManager.Instance.TriggerEvent(new EventData.CLaimGroundTile()
            {
                ClaimPos = GetListBuildingPosition(buildingDic),
                TileType = TileType.Blocker
            });
        }

        private bool IsValidPosition(Vector3 position, float gapDistance, Dictionary<int, BuildingBase> buildingDic)
        {
            var compareDistance = gapDistance * gapDistance;
            foreach (var building in buildingDic)
            {
                if ((building.Value.Position - position).sqrMagnitude < compareDistance)
                {
                    return false;
                }
            }
            return true;
        }

        public void DespawnRewardConstruct(int id, UnityAction<Vector3> despawnRewardConstructSuccess)
        {
            if (rewardBuildingDic.Remove(id, out var building))
            {
                despawnRewardConstructSuccess?.Invoke(building.Position);
            }
        }

        private List<Vector3> GetListBuildingPosition(Dictionary<int, BuildingBase> buildingDic)
        {
            var posList = new List<Vector3>(buildingDic.Count);
            foreach (var building in buildingDic)
            {
                posList.Add(building.Value.Position);
            }
            return posList;
        }
    }
}
