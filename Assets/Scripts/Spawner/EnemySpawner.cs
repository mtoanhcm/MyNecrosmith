using Character;
using Config;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawner{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemyConfig enemyNeedSpawnConfig;
        private bool isInit;

        [Button]
        private void TestSpawnEnemy(EnemyConfig config)
        {
            if (!isInit)
            {
                Init(config);
            }
            
            SpawnEnemy();
        }
        
        public void Init(EnemyConfig config)
        {
            enemyNeedSpawnConfig = config;
            isInit = true;
        }

        public void SpawnEnemy()
        {
            if (!isInit)
            {
                return;
            }

            var data = new EventData.OnSpawnEnemy()
            {
                EnemyID = enemyNeedSpawnConfig.ID.ToString(),
                OnSpawnSuccess = OnSpawnEnemy
            };

            EventManager.Instance.TriggerEvent(data);

            return;

            void OnSpawnEnemy(CharacterBase character)
            {
                var enemy = character as EnemyCharacter;
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
