using System;
using Building;
using Observer;
using Pool;
using UnityEngine;

namespace Spawner
{
    public class BuildingObjectPool : MonoBehaviour
    {
        private ObjectPool<BuildingBase> pool;

        private void Awake()
        {
            pool = new ObjectPool<BuildingBase>(transform);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnSpawnBuilding>(OnSpawnBuilding);
            EventManager.Instance.StartListening<EventData.OnDespawnBuilding>(OnDespawnBuilding);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OnSpawnBuilding>(OnSpawnBuilding);
            EventManager.Instance?.StopListening<EventData.OnDespawnBuilding>(OnDespawnBuilding);
            
            pool.Dispose();
        }

        private async void OnSpawnBuilding(EventData.OnSpawnBuilding data)
        {
            var building = await pool.Get($"Building/{data.BuildingID}.prefab");
            if (building == null)
            {
                Debug.LogWarning($"Cannot spawn enemy {data.BuildingID}");
                return;
            }
            
            data.OnSpawnSuccess?.Invoke(building);
        }

        private void OnDespawnBuilding(EventData.OnDespawnBuilding data)
        {
            pool.Return($"Building/{data.Building.BuildingID}.prefab", data.Building);
        }
    }   
}
