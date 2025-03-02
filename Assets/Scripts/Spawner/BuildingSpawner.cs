using System;
using Building;
using Observer;
using UnityEngine;

namespace Spawner
{
    public class BuildingSpawner : MonoBehaviour
    {
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
            }
        }
    }
}
