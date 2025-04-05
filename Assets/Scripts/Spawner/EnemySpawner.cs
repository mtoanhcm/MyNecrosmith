using Character;
using Config;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawner{
    public class EnemySpawner : MonoBehaviour
    {
        private bool isInit;
        
        public void Init()
        {
            isInit = true;
        }

        public void SpawnEnemy(EnemyConfig config)
        {
            if (!isInit)
            {
                return;
            }

            var data = new EventData.OnSpawnEnemy()
            {
                EnemyID = config.ID.ToString(),
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

                enemy.Spawn(new EnemyData(config));
                enemy.transform.position = transform.position;
            }
        }
    }
}
