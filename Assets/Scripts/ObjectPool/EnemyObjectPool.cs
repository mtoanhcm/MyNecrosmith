using System;
using System.Threading.Tasks;
using Character;
using Observer;
using Pool;
using UnityEngine;

namespace Spawner
{
    public class EnemyObjectPool : MonoBehaviour
    {
        private ObjectPool<CharacterBase> pool;

        private void Awake()
        {
            pool = new ObjectPool<CharacterBase>(transform);
            
            EventManager.Instance.StartListening<EventData.OnSpawnEnemy>(OnSpawnEnemy);
            EventManager.Instance.StartListening<EventData.OnEnemyDeath>(OnDespawnEnemy);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OnSpawnEnemy>(OnSpawnEnemy);
            EventManager.Instance?.StopListening<EventData.OnEnemyDeath>(OnDespawnEnemy);
            
            pool.Dispose();
        }

        private async void OnSpawnEnemy(EventData.OnSpawnEnemy data)
        {
            var enemy = await pool.Get($"CharacterBase/Enemy/{data.EnemyID}.prefab");
            if (enemy == null)
            {
                Debug.LogWarning($"Cannot spawn enemy {data.EnemyID}");
                return;
            }
            
            data.OnSpawnSuccess?.Invoke(enemy);
        }
        
        private void OnDespawnEnemy(EventData.OnEnemyDeath data)
        {
            pool.Return($"CharacterBase/Enemy/{data.Enemy.Data.ID}.prefab", data.Enemy);
        }
    }   
}
