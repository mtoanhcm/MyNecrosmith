using System;
using System.Collections.Generic;
using Combat;
using Config;
using GameUtility;
using Spawner;
using UnityEngine;

namespace Training
{
    [Serializable]
    public struct MinionInfo
    {
        public CharacterID minionID;
        public List<EquipmentID> equipmentIDs;
    }

    public class TrainingController : MonoBehaviour
    {
        [Header("Method")]
        [SerializeField] private bool autoSpawn;
        [SerializeField] private MinionInfo autoSpawnMinionInfo;
        [SerializeField] private CharacterID autoSpawnEnemyID;
        private float tempDelayTimeAutoSpawn;

        [Header("Spawner")]
        [SerializeField] private MinionSpawner minionSpawner;
        [SerializeField] private EnemySpawner enemySpawner;

        private void Start()
        {
            DamageReduction.Init();
        }

        private void Update()
        {
            if (autoSpawn) {
                ProgressAutoSpawn();
                return;
            }
        }

        private void ProgressAutoSpawn()
        {
            if(autoSpawnMinionInfo.minionID == CharacterID.None || autoSpawnEnemyID == CharacterID.None)
            {
                return;
            }

            if(tempDelayTimeAutoSpawn > Time.time)
            {
                return;
            }

            tempDelayTimeAutoSpawn = Time.time + 0.5f;

            SendRequestAutoSpawnEnemy();
        }

        private async void SendRequestAutoSpawnEnemy()
        {
            var path = $"Config/Character/Enemy/{autoSpawnEnemyID}.asset";
            var tempConfig = await AddressableUtility.LoadAssetAsync<EnemyConfig>(path);
            if (tempConfig == null)
            {
                Debug.LogError($"Cannot find enemy {autoSpawnEnemyID} config");
                return;
            }

            enemySpawner.SpawnEnemy(tempConfig);
        }
    }
}
