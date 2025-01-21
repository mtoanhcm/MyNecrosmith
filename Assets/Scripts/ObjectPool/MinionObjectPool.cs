using System;
using Character;
using Observer;
using Pool;
using UnityEngine;

namespace Spawner
{
    public class MinionObjectPool : MonoBehaviour
    {
        private ObjectPool<CharacterBase> pool;

        private void Awake()
        {
            pool = new ObjectPool<CharacterBase>(transform);
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnSpawnMinion>(OnSpawnMinion);
            EventManager.Instance.StartListening<EventData.OnMinionDeath>(OnMinionDeath);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OnSpawnMinion>(OnSpawnMinion);
            EventManager.Instance?.StopListening<EventData.OnMinionDeath>(OnMinionDeath);
            
            pool.Dispose();
        }

        private async void OnSpawnMinion(EventData.OnSpawnMinion data)
        {
            var minion = await pool.Get($"Character/Minion/{data.MinionID}.prefab");
            if (minion == null)
            {
                Debug.LogWarning($"Cannot spawn minion {data.MinionID}");
                return;
            }
            
            data.OnSpawnSuccess?.Invoke(minion);
        }

        private void OnMinionDeath(EventData.OnMinionDeath data)
        {
            pool.Return($"Character/Minion/{data.Minion.Data.ID}.prefab", data.Minion);
        }
    }
}
