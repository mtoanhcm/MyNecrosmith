using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Character;
using Config;
using Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace Spawner
{
    public class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance;
        
        // Caches to store operation handles
        private Dictionary<string, AsyncOperationHandle<GameObject>> characterHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
        private Dictionary<string, AsyncOperationHandle<GameObject>> equipmentHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
        private Dictionary<string, AsyncOperationHandle<SpriteAtlas>> atlasHandles = new Dictionary<string, AsyncOperationHandle<SpriteAtlas>>();


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else if(Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            // Release all addressable handles when destroyed
            foreach (var handle in characterHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            foreach (var handle in equipmentHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            foreach (var handle in atlasHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
        }
        
        #region Character Prefab Loading

        /// <summary>
        /// Asynchronously loads a character prefab based on the specified side and class.
        /// </summary>
        /// <param name="characterSide">The side of the character.</param>
        /// <param name="characterClass">The class of the character.</param>
        /// <returns>The loaded CharacterBase or null if failed.</returns>
        public async Task<CharacterBase> LoadCharacterPrefabAsync(string characterSide, string characterClass)
        { 
            string address = $"Character/{characterSide}/{characterClass}.prefab";
            string cacheKey = $"{characterSide}_{characterClass}";

            // Check if we already have a valid handle in the cache
            if (characterHandles.TryGetValue(cacheKey, out var cachedHandle) && cachedHandle.IsValid())
            {
                Debug.Log($"Character Prefab '{characterClass}' retrieved from cache.");
                return cachedHandle.Result.GetComponent<CharacterBase>();
            }

            try
            {
                // Load the prefab using Addressables
                var handle = Addressables.LoadAssetAsync<GameObject>(address);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Cache the handle
                    characterHandles[cacheKey] = handle;
                    Debug.Log($"Character Prefab '{characterClass}' loaded successfully.");
                    return handle.Result.GetComponent<CharacterBase>();
                }
                else
                {
                    Debug.LogError($"Failed to load Character Prefab with address: {address}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading character prefab: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Unloads a specific character prefab.
        /// </summary>
        /// <param name="characterSide">The side of the character.</param>
        /// <param name="characterClass">The class of the character to unload.</param>
        public void UnloadCharacterPrefab(string characterSide, string characterClass)
        {
            string cacheKey = $"{characterSide}_{characterClass}";
            
            if (characterHandles.TryGetValue(cacheKey, out var handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
                
                characterHandles.Remove(cacheKey);
                Debug.Log($"Character Prefab '{characterClass}' unloaded successfully.");
            }
        }

        #endregion

        #region Equipment Prefab Loading

        /// <summary>
        /// Asynchronously loads an equipment prefab based on the specified type.
        /// </summary>
        /// <param name="equipmentCategory">The category of equipment.</param>
        /// <param name="equipmentType">The type of the equipment.</param>
        /// <returns>The loaded EquipmentBase or null if failed.</returns>
        public async Task<EquipmentBase> LoadEquipmentPrefabAsync(string equipmentCategory, string equipmentType)
        {
            string address = $"Equipment/{equipmentCategory}/{equipmentType}.prefab";
            string cacheKey = $"{equipmentCategory}_{equipmentType}";

            // Check if we already have a valid handle in the cache
            if (equipmentHandles.TryGetValue(cacheKey, out var cachedHandle) && cachedHandle.IsValid())
            {
                Debug.Log($"Equipment Prefab '{equipmentType}' retrieved from cache.");
                return cachedHandle.Result.GetComponent<EquipmentBase>();
            }

            try
            {
                // Load the prefab using Addressables
                var handle = Addressables.LoadAssetAsync<GameObject>(address);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Cache the handle
                    equipmentHandles[cacheKey] = handle;
                    Debug.Log($"Equipment Prefab '{equipmentType}' loaded successfully.");
                    return handle.Result.GetComponent<EquipmentBase>();
                }
                else
                {
                    Debug.LogError($"Failed to load Equipment Prefab with address: {address}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading equipment prefab: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Unloads a specific equipment prefab.
        /// </summary>
        /// <param name="equipmentCategory">The category of equipment.</param>
        /// <param name="equipmentType">The type of the equipment to unload.</param>
        public void UnloadEquipmentPrefab(string equipmentCategory, string equipmentType)
        {
            string cacheKey = $"{equipmentCategory}_{equipmentType}";
            
            if (equipmentHandles.TryGetValue(cacheKey, out var handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
                
                equipmentHandles.Remove(cacheKey);
                Debug.Log($"Equipment Prefab '{equipmentType}' unloaded successfully.");
            }
        }

        #endregion

        #region Sprite Atlas Loading

        /// <summary>
        /// Asynchronously loads a Sprite Atlas based on the specified address.
        /// </summary>
        /// <param name="atlasAddress">The address of the Sprite Atlas.</param>
        /// <returns>The loaded SpriteAtlas or null if failed.</returns>
        public async Task<SpriteAtlas> LoadSpriteAtlasAsync(string atlasAddress)
        {
            // Check if we already have a valid handle in the cache
            if (atlasHandles.TryGetValue(atlasAddress, out var cachedHandle) && cachedHandle.IsValid())
            {
                Debug.Log($"Sprite Atlas '{atlasAddress}' retrieved from cache.");
                return cachedHandle.Result;
            }

            try
            {
                // Load the atlas using Addressables
                var handle = Addressables.LoadAssetAsync<SpriteAtlas>(atlasAddress);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Cache the handle
                    atlasHandles[atlasAddress] = handle;
                    Debug.Log($"Sprite Atlas '{atlasAddress}' loaded successfully.");
                    return handle.Result;
                }
                else
                {
                    Debug.LogError($"Failed to load Sprite Atlas with address: {atlasAddress}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading sprite atlas: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Unloads a specific Sprite Atlas.
        /// </summary>
        /// <param name="atlasAddress">The address of the Sprite Atlas to unload.</param>
        public void UnloadSpriteAtlas(string atlasAddress)
        {
            if (atlasHandles.TryGetValue(atlasAddress, out var handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
                
                atlasHandles.Remove(atlasAddress);
                Debug.Log($"Sprite Atlas '{atlasAddress}' unloaded successfully.");
            }
        }

        #endregion

        #region Configuration Loading

        /// <summary>
        /// Asynchronously loads an equipment configuration.
        /// </summary>
        /// <param name="equipmentCategory">Category of the equipment</param>
        /// <param name="equipmentID">ID of the equipment</param>
        /// <returns>The equipment configuration or null if failed</returns>
        public async Task<EquipmentConfig> LoadEquipmentConfigAsync(string equipmentCategory, string equipmentID)
        {
            string address = $"Config/Equipment/{equipmentCategory}/{equipmentID}";
            
            try
            {
                var handle = Addressables.LoadAssetAsync<EquipmentConfig>(address);
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    EquipmentConfig config = handle.Result;
                    Addressables.Release(handle); // Release immediately as we don't need to keep the asset loaded
                    return config;
                }
                else
                {
                    Debug.LogError($"Failed to load equipment config: {address}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading equipment config: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Asynchronously loads a character configuration.
        /// </summary>
        /// <param name="characterType">Type of character (Minion, Enemy, etc.)</param>
        /// <param name="characterID">ID of the character</param>
        /// <returns>The character configuration or null if failed</returns>
        public async Task<CharacterConfig> LoadCharacterConfigAsync(string characterType, string characterID)
        {
            string address = $"Config/Character/{characterType}/{characterID}";
            
            try
            {
                var handle = Addressables.LoadAssetAsync<CharacterConfig>(address);
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    CharacterConfig config = handle.Result;
                    Addressables.Release(handle); // Release immediately as we don't need to keep the asset loaded
                    return config;
                }
                else
                {
                    Debug.LogError($"Failed to load character config: {address}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading character config: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region General Unload Methods

        /// <summary>
        /// Unloads all loaded assets managed by the ResourcesManager.
        /// </summary>
        public void UnloadAllAssets()
        {
            // Unload Character Prefabs
            foreach (var handle in characterHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            characterHandles.Clear();

            // Unload Equipment Prefabs
            foreach (var handle in equipmentHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            equipmentHandles.Clear();

            // Unload Sprite Atlases
            foreach (var handle in atlasHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            atlasHandles.Clear();

            Debug.Log("All assets unloaded successfully.");
        }

        #endregion
    }
}