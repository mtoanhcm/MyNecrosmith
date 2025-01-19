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
        [SerializeField] private CharacterID enemyNeedSpawns;

        private CharacterBase enemyCharacter;
        
        private EnemyConfig config;
        
        private bool isInit;

        private void Awake()
        {
            
        }
        
        private void Start()
        {
            PrepareEnemyConfig();
        }

        private CharacterBase CreateObjectForPool(string typeID)
        {
            return Instantiate(enemyCharacter, transform);
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
            // var enemy = objectPool.GetObject(enemyNeedSpawns.ToString());
            // if (enemy == null)
            // {
            //     Debug.LogError("Cannot instantiate minion character");
            //     return;
            // }
            //
            // enemy.Spawn(new EnemyData(config));
            // enemy.transform.position = transform.position;
        }
    }
}
