using Character;
using Config;
using GameUtility;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Spawner{
    public class EnemySpawner : ObjectSpawner<CharacterBase>
    {
        [SerializeField] private AssetReference characterBaseRef;
        [SerializeField] private CharacterID enemyNeedSpawns;

        private CharacterBase enemyCharacter;
        
        private EnemyConfig config;
        
        private bool isInit;

        protected override void Start()
        {
            base.Start();
            
            PrepareEnemyConfig();
            PrepareEnemyPrefab();
        }

        protected override CharacterBase CreateObjectForPool(string typeID)
        {
            return Instantiate(enemyCharacter, transform);
        }
        
        private async void PrepareEnemyPrefab()
        {
            var characterObj = await AddressableUtility.LoadAssetAsync<GameObject>(characterBaseRef);
            if (characterObj == null)
            {
                Debug.LogError($"Cannot load character base");
                return;
            }
            
            enemyCharacter = characterObj.GetComponent<EnemyCharacter>();
            if (enemyCharacter == null)
            {
                Debug.LogError($"Cannot parse character base to minion character");
            }
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
            var enemy = objectPool.GetObject(enemyNeedSpawns.ToString());
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
