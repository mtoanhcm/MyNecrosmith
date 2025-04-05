using System;
using Config;
using GameUtility;
using Observer;
using Spawner;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UISpawnEnemyButton : MonoBehaviour
    {
        [SerializeField] private CharacterID enemyID;
        [SerializeField] private EnemySpawner spawner;
        
        private Button myBtn;
        private EnemyConfig tempConfig;
        
        private void Awake()
        {
            if (TryGetComponent(out myBtn))
            {
                myBtn.onClick.RemoveAllListeners();
                myBtn.onClick.AddListener(SendRequestSpawnEnemy);
            }
        }

        private void Start()
        {
            spawner.Init();
        }

        private async void SendRequestSpawnEnemy()
        {
            if (tempConfig == null || tempConfig.ID != enemyID)
            {
                var path = $"Config/Character/Enemy/{enemyID}.asset";
                tempConfig = await AddressableUtility.LoadAssetAsync<EnemyConfig>(path);
                if (tempConfig == null)
                {
                    Debug.LogError($"Cannot find enemy {enemyID} config");
                    return;
                }
            }
            
            spawner.SpawnEnemy(tempConfig);
        }
    }   
}
