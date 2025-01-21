using Equipment;
using Observer;
using UnityEngine;
using Pool;

namespace Spawner
{
    public class ItemObjectPool : MonoBehaviour
    {
        private ObjectPool<EquipmentBase> pool;

        private void Awake()
        {
            pool = new ObjectPool<EquipmentBase>(transform);
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnSpawnEquipment>(OnSpawnEquipment);
            EventManager.Instance.StartListening<EventData.OnDestroyEquipment>(OnDespawnEquipment);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OnSpawnEquipment>(OnSpawnEquipment);
            EventManager.Instance?.StopListening<EventData.OnDestroyEquipment>(OnDespawnEquipment);
            
            pool.Dispose();
        }

        private async void OnSpawnEquipment(EventData.OnSpawnEquipment data)
        {
            var equipment = await pool.Get($"Equipment/{data.EquipmentID}.prefab");
            if (equipment == null)
            {
                Debug.LogWarning($"Cannot spawn equipment {data.EquipmentID}");
                return;
            }
            
            data.OnSpawnEquipmentSuccessHandle?.Invoke(equipment);
        }
        
        private void OnDespawnEquipment(EventData.OnDestroyEquipment data)
        {
            pool.Return($"Equipment/{data.Equipment.Data.ID}.prefab", data.Equipment);
        }
    }   
}
