using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using Cysharp.Threading.Tasks;
using Observer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Equipment.Drop
{
    public class EquipmentSpawner : MonoBehaviour
    {
        private Dictionary<string, AsyncOperationHandle<EquipmentConfig>> configHandles = new Dictionary<string, AsyncOperationHandle<EquipmentConfig>>();
        
        private void Awake()
        {
            // Initialize dictionary
        }

        private void OnDestroy()
        {
            // Release all addressable handles when destroyed
            foreach (var handle in configHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            configHandles.Clear();
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnEnemyDeath>(OnEnemyDeath);
            TestAddEquipment().Forget();
        }

        private async UniTaskVoid TestAddEquipment()
        {
            await UniTask.Delay(1000);
            
            var config = await GetEquipmentConfig("Sword", "Sword") as WeaponConfig;
            if (config != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment(){ EquipmentData = new WeaponData(config)});
            }
            else
            {
                Debug.Log($"Cannot find config for equipment");
            }
        }
        
        private async void OnEnemyDeath(EventData.OnEnemyDeath data)
        {
            var config = await GetEquipmentConfig("Sword", "Sword") as WeaponConfig;
            if (config != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment(){ EquipmentData = new WeaponData(config)});
            }
            else
            {
                Debug.Log($"Cannot find config for equipment");
            }
        }

        private async Task<EquipmentConfig> GetEquipmentConfig(string equipmentCategory, string equipmentName)
        {
            string cacheKey = $"{equipmentCategory}_{equipmentName}";
            
            // Check if we already have a valid handle in the cache
            if (configHandles.TryGetValue(cacheKey, out var cachedHandle) && cachedHandle.IsValid())
            {
                Debug.Log($"Equipment Config '{equipmentName}' retrieved from cache.");
                return cachedHandle.Result;
            }
            
            try
            {
                // Load the config using Addressables
                string address = $"Config/Equipment/{equipmentCategory}/{equipmentName}.asset";
                var handle = Addressables.LoadAssetAsync<EquipmentConfig>(address);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Cache the handle
                    configHandles[cacheKey] = handle;
                    Debug.Log($"Equipment Config '{equipmentName}' loaded successfully.");
                    return handle.Result;
                }
                else
                {
                    Debug.LogError($"Failed to load Equipment Config with address: {address}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading equipment config: {ex.Message}");
                return null;
            }
        }
    }   
}