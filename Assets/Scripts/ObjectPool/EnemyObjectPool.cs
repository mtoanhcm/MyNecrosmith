using Character;
using GameUtility;
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
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnSpawnEnemy>(OnSpawnEnemy);
            EventManager.Instance.StartListening<EventData.OnEnemyDeath>(OnDespawnEnemy);
        }

        private void OnDestroy()
        {
            if (EventManager.HasInstance)
            {
                EventManager.Instance?.StopListening<EventData.OnSpawnEnemy>(OnSpawnEnemy);
                EventManager.Instance?.StopListening<EventData.OnEnemyDeath>(OnDespawnEnemy);
            }
            
            pool.Dispose();
        }

        private async void OnSpawnEnemy(EventData.OnSpawnEnemy data)
        {
            var enemy = await pool.Get($"Character/Enemy/pref_{data.EnemyID}.prefab");
            if (enemy == null)
            {
                Debug.LogWarning($"Cannot spawn enemy {data.EnemyID}");
                return;
            }

            enemy.SetActive(false);
            data.OnSpawnSuccess?.Invoke(enemy);
        }
        
        private void OnDespawnEnemy(EventData.OnEnemyDeath data)
        {
            Debug.Log($"Despawn enemy {data.Enemy.Data.ID}");
            pool.Return($"Character/Enemy/pref_{data.Enemy.Data.ID}.prefab", data.Enemy);
        }
    }   
}
