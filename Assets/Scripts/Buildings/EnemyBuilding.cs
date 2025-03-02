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
            
            InitSpawner();
        }

        protected override void PlayActivation()
        {
            throw new System.NotImplementedException();
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
