using Observer;
using Spawner;
using UnityEngine;

namespace Building
{
    public class EnemyBuilding : BuildingBase
    {
        [SerializeField] private EnemySpawner enemySpawner;
        
        private EnemyBuildingData enemyBuildingData;

        public override void Spawn(Vector3 pos, BuildingData initData)
        {
            base.Spawn(pos, initData);
            enemyBuildingData = initData as EnemyBuildingData;

            if (enemyBuildingData != null)
            {
                activationCooldownTime = enemyBuildingData.CooldownSpawnTime;
                //Debug.Log($"Set enemy building spawn time delay: {activationCooldownTime} --- {enemyBuildingData.CooldownSpawnTime}");
                InitSpawner();
            }
        }

        protected override void PlayActivation()
        {
            enemySpawner.SpawnEnemy();
        }

        protected override void OnBuildingClaimed()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnDespawnBuilding() { Building = this});
        }

        private void InitSpawner()
        {
            if (enemySpawner == null)
            {
                Debug.LogError($"Building {name} does not has spawner");
                return;
            }
            
            enemySpawner.Init(enemyBuildingData.EnemySpawnConfig);
        }
    }
}
