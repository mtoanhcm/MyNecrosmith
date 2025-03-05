using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using Observer;
using UnityEngine;

namespace Spawner
{
    public class BuildingSpawner : MonoBehaviour
    {
        private Dictionary<int, List<BuildingBase>> buildings = new Dictionary<int, List<BuildingBase>>();
        
        public void SpawnBuilding(BuildingData buildingData, Vector3 position)
        {
            var data = new EventData.OnSpawnBuilding()
            {
                BuildingID = buildingData.ID,
                OnSpawnSuccess = OnSpawnBuildingSuccess
            };
            
            EventManager.Instance.TriggerEvent(data);
            return;

            void OnSpawnBuildingSuccess(BuildingBase building)
            {
                building.Spawn(position, buildingData);
                AddBuildingToBuildingList(building);
            }
        }

        public List<BuildingBase> GetBuildings(int areaIndex)
        {
            if (buildings == null || buildings.Count == 0 || areaIndex < 0 || areaIndex >= buildings.Count)
            {
                return null;
            }
            
            return buildings[areaIndex];
        }
        
        public List<BuildingBase> GetBuildings(int areaIndex, int amount, Vector3 originPos)
        {
            if (buildings == null || buildings.Count == 0 || areaIndex < 0 || areaIndex >= buildings.Count)
            {
                return new List<BuildingBase>();
            }

            if (amount <= 0)
            {
                return new List<BuildingBase>();
            }

            amount = Mathf.Min(amount, buildings[areaIndex].Count);
            
            return buildings[areaIndex]
                .OrderBy(go => (go.transform.position - originPos).sqrMagnitude)
                .Take(amount)
                .ToList();
        }

        private void AddBuildingToBuildingList(BuildingBase building)
        {
            if (!buildings.ContainsKey(building.AreaIndex))
            {
                buildings[building.AreaIndex] = new List<BuildingBase>();
            }
            
            buildings[building.AreaIndex].Add(building);
        }
    }
}
