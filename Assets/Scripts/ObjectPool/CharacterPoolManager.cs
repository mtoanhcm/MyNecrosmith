using Character;
using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class CharacterPoolManager : MonoBehaviour
    {
        public static CharacterPoolManager Instance;

        private ObjectPool<CharacterBase> enemyPool;
        private ObjectPool<CharacterBase> minionPool;

        private void Awake()
        {
            if (Instance == null) { 
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            var enemyPrefab = Resources.Load<CharacterBase>("SimpleEnemy");
            enemyPool = new ObjectPool<CharacterBase>(enemyPrefab, 50, 20, transform);
            var minionPrefab = Resources.Load<CharacterBase>("Minion");
            minionPool = new ObjectPool<CharacterBase>(minionPrefab, 50, 20, transform);
        }

        public CharacterBase SpawnEnemy()
        {
            return enemyPool.Get();
            // Set the enemy position, etc.
        }

        public void ReturnEnemyToPool(CharacterBase enemy)
        {
            enemyPool.ReturnToPool(enemy);
        }

        public CharacterBase SpawnMinion() { 
            return minionPool.Get();
        }

        public void ReturnMinionToPool(CharacterBase minion)
        {
            minionPool.ReturnToPool(minion);
        }
    }
}
