using System.Collections;
using Character;
using Config;
using GameUtility;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Spawner{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyConfig enemyNeedSpawnConfig;
        
        [Button]
        public void SpawnEnemy()
        {
            EventData.OnSpawnEnemy data = new EventData.OnSpawnEnemy()
            {
                EnemyID = enemyNeedSpawnConfig.ID.ToString(),
                OnSpawnSuccess = OnSpawnEnemy
            };
            
            EventManager.Instance.TriggerEvent(data);

            void OnSpawnEnemy(CharacterBase enemy)
            {
                if (enemy == null)
                {
                    Debug.LogError("Cannot instantiate minion character");
                    return;
                }
            
                enemy.Spawn(new EnemyData(enemyNeedSpawnConfig));
                enemy.transform.position = transform.position;
            }
        }
    }
}
