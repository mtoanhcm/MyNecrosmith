using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using Equipment;
using GameUtility;
using Observer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay
{
    public class PlayerManager : MonoBehaviour
    {
        [Serializable]
        public struct EquipmentInitData
        {
            public EquipmentID EquipmentID;
            public EquipmentCategoryID Category;
            public int Amount;
        }

        [SerializeField] 
        private EquipmentInitData[] initEquipment;
        
        private GameRuntimeData runtimeData;
        private Dictionary<string, AsyncOperationHandle<EquipmentConfig>> configHandles = new Dictionary<string, AsyncOperationHandle<EquipmentConfig>>();
        
        private void Awake()
        {
            // Load the runtime data using Addressables
            LoadRuntimeData();
            
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
            EventManager.Instance.StartListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipment);
        }

        private async void LoadRuntimeData()
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<GameRuntimeData>("GameRuntimeData");
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    runtimeData = handle.Result;
                    
                    #if UNITY_EDITOR
                    if (runtimeData != null)
                    {
                        runtimeData.Reset();
                    }
                    #endif
                    
                    // Initialize startup equipment after runtime data is loaded
                    InitStartupEquipment();
                }
                else
                {
                    Debug.LogError("Failed to load GameRuntimeData");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading GameRuntimeData: {ex.Message}");
            }
        }

        private void OnDestroy()
        {
            // Release all addressable handles
            foreach (var handle in configHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            EventManager.Instance?.StopListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
            EventManager.Instance?.StopListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipment);
        }

        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            if (runtimeData == null)
            {
                Debug.LogError("Runtime data not loaded yet");
                return;
            }
            
            runtimeData.AddEquipmentToStorage(data.EquipmentData);
            SendEventEquipmentStorageChanged();
        }

        private void OnRemoveEquipment(EventData.OnRemoveEquipmentFromPlayerStorage data)
        {
            if (runtimeData == null)
            {
                Debug.LogError("Runtime data not loaded yet");
                return;
            }
            
            runtimeData.RemoveEquipmentFromStorage(data.EquipmentID);
            SendEventEquipmentStorageChanged();
        }

        private void SendEventEquipmentStorageChanged()
        {
            if (runtimeData == null) return;
            
            EventManager.Instance.TriggerEvent(new EventData.OnEquipmentStorageChanged()
            {
                Equipment = runtimeData.EquipmentStorage
            });
        }

        private async void InitStartupEquipment()
        {
            if (runtimeData == null || initEquipment == null) return;
            
            for (var i = 0; i < initEquipment.Length; i++)
            {
                var equipment = initEquipment[i];
                for (var j = 0; j < equipment.Amount; j++)
                {
                    // Load the config using Addressables
                    var config = await LoadEquipmentConfig(equipment.Category.ToString(), equipment.EquipmentID.ToString());
                    
                    if (config == null)
                    {
                        Debug.LogError($"Could not load equipment {equipment.EquipmentID} from Addressables");
                        continue;
                    }

                    if (AddWeaponEquipment(config))
                    {
                        continue;
                    }
                }
            }
        }

        private async Task<EquipmentConfig> LoadEquipmentConfig(string categoryName, string equipmentName)
        {
            string cacheKey = $"{categoryName}_{equipmentName}";
            
            // Check if we already have a valid handle in the cache
            if (configHandles.TryGetValue(cacheKey, out var cachedHandle) && cachedHandle.IsValid())
            {
                return cachedHandle.Result;
            }
            
            try
            {
                // Load the config using Addressables
                string address = $"Config/Equipment/{categoryName}/{equipmentName}.asset";
                var handle = Addressables.LoadAssetAsync<EquipmentConfig>(address);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Cache the handle
                    configHandles[cacheKey] = handle;
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

        private bool AddWeaponEquipment(EquipmentConfig config)
        {
            if (config.CategoryID.IsWeaponType())
            {
                runtimeData.AddEquipmentToStorage(new WeaponData(config));
                return true;
            }

            return false;
        }
    }   
}