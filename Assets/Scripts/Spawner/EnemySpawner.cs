using Character;
using Config;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawner{
    public class EnemySpawner : ObjectSpawner<EnemyCharacter>
    {
        [SerializeField] private CharacterID enemyNeedSpawns;
        private EnemyConfig config;
        
        private bool isInit;

        protected override void Start()
        {
            base.Start();
            
            PrepareEnemyConfig();
            PrepareEnemyPrefab();
        }

        private async void PrepareEnemyPrefab()
        {
            var characterBase = await ResourcesManager.Instance.LoadCharacterPrefabAsync("Enemy", enemyNeedSpawns.ToString());
            var enemyPrefab = characterBase as EnemyCharacter;
            if (enemyPrefab == null)
            {
                Debug.LogError($"Cannot load {enemyNeedSpawns} prefab");
                return;
            }
            
            prefabDictionary.Add(enemyNeedSpawns.ToString(), enemyPrefab);
            isInit = true;
        }

        private void PrepareEnemyConfig()
        {
            config = Resources.Load<EnemyConfig>($"Character/Enemy/{enemyNeedSpawns}");
            if (config == null)
            {
                Debug.LogError($"Cannot load {enemyNeedSpawns} config");
            }
        }
        
        [Button]
        public void SpawnEnemy()
        {
            if (isInit)
            {
                return;
            }
            
            var enemy = objectPool.GetObject(enemyNeedSpawns.ToString()) as EnemyCharacter;
            if (enemy == null)
            {
                Debug.LogError("Cannot instantiate minion character");
                return;
            }

            enemy.Spawn(new MinionData(config));
            enemy.transform.position = transform.position;
        }
    }
}
