using System;
using System.Collections.Generic;
using Combat;
using Config;
using Equipment;
using GameUtility;
using Spawner;
using UnityEngine;

namespace Training
{
    [Serializable]
    public struct MinionInfo
    {
        public CharacterID minionID;
        public EquipmentConfig[] equipmentConfigs;
    }

    public class TrainingController : MonoBehaviour
    {
        [Header("Method")]
        [SerializeField] private bool autoSpawn;
        [SerializeField] private MinionInfo autoSpawnMinionInfo;
        [SerializeField] private CharacterID autoSpawnEnemyID;
        [SerializeField] private float delayAutoSpawnTime;
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

            tempDelayTimeAutoSpawn = Time.time + delayAutoSpawnTime;

            SendRequestAutoSpawnMinion();
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

        private async void SendRequestAutoSpawnMinion()
        {
            var path = $"Config/Character/Minion/{autoSpawnMinionInfo.minionID}.asset";
            var tempConfig = await AddressableUtility.LoadAssetAsync<MinionConfig>(path);
            if (tempConfig == null)
            {
                Debug.LogError($"Cannot find minion {autoSpawnMinionInfo.minionID} config");
                return;
            }

            var equipmentConfigs = autoSpawnMinionInfo.equipmentConfigs;
            var euipmentDatas = new EquipmentData[equipmentConfigs.Length];
            for(var i =0; i < equipmentConfigs.Length; i++)
            {
                var config = equipmentConfigs[i];
                euipmentDatas[i] = config.Group switch
                {
                    EquipmentGroup.Weapon => new WeaponData(config),
                    EquipmentGroup.Armor => new ArmorData(config),
                    _ => null
                };
            }


            minionSpawner.SpawnMinion(tempConfig, euipmentDatas);
        }
    }
}
