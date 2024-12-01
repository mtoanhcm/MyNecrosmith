using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Character;
using Equipment;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace Spawner
{
    public class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance;
        
        // Caches to store loaded assets
        private Dictionary<string, CharacterBase> characterPrefabs = new Dictionary<string, CharacterBase>();
        private Dictionary<string, EquipmentBase> equipmentPrefabs = new Dictionary<string, EquipmentBase>();
        private Dictionary<string, SpriteAtlas> spriteAtlases = new Dictionary<string, SpriteAtlas>();


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
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

            if (characterPrefabs.TryGetValue(characterClass, out var cachedPrefab))
            {
                Debug.Log($"Character Prefab '{characterClass}' retrieved from cache.");
                return cachedPrefab;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            CharacterBase characterPrefab = null;

            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded && operation.Result.TryGetComponent(out characterPrefab))
                {
                    characterPrefabs[characterClass] = characterPrefab;
                    Debug.Log($"Character Prefab '{characterClass}' loaded successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to load Character Prefab with address: {address}");
                }
            };

            // Monitor progress
            while (!handle.IsDone)
            {
                // Optionally, implement progress reporting here
                await Task.Yield(); // Wait for the next frame
            }

            return characterPrefab;
        }

        /// <summary>
        /// Unloads a specific character prefab.
        /// </summary>
        /// <param name="characterClass">The class of the character to unload.</param>
        public void UnloadCharacterPrefab(string characterClass)
        {
            if (characterPrefabs.ContainsKey(characterClass))
            {
                Addressables.Release(characterPrefabs[characterClass].gameObject);
                characterPrefabs.Remove(characterClass);
                Debug.Log($"Character Prefab '{characterClass}' unloaded successfully.");
            }
        }

        #endregion

        #region Equipment Prefab Loading

        /// <summary>
        /// Asynchronously loads an equipment prefab based on the specified type.
        /// </summary>
        /// <param name="equipmentType">The type of the equipment.</param>
        /// <returns>The loaded EquipmentBase or null if failed.</returns>
        public async Task<EquipmentBase> LoadEquipmentPrefabAsync(string equipmentType)
        {
            string address = $"Equipment/{equipmentType}.prefab";

            if (equipmentPrefabs.TryGetValue(equipmentType, out var cachedPrefab))
            {
                Debug.Log($"Equipment Prefab '{equipmentType}' retrieved from cache.");
                return cachedPrefab;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            EquipmentBase equipmentPrefab = null;
            
            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded && operation.Result.TryGetComponent(out equipmentPrefab))
                {
                    equipmentPrefabs[equipmentType] = equipmentPrefab;
                    Debug.Log($"Equipment Prefab '{equipmentType}' loaded successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to load Equipment Prefab with address: {address}");
                }
            };

            // Monitor progress
            while (!handle.IsDone)
            {
                // Optionally, implement progress reporting here
                await Task.Yield(); // Wait for the next frame
            }

            return equipmentPrefab;
        }

        /// <summary>
        /// Unloads a specific equipment prefab.
        /// </summary>
        /// <param name="equipmentType">The type of the equipment to unload.</param>
        public void UnloadEquipmentPrefab(string equipmentType)
        {
            if (equipmentPrefabs.ContainsKey(equipmentType))
            {
                Addressables.Release(equipmentPrefabs[equipmentType].gameObject);
                equipmentPrefabs.Remove(equipmentType);
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
            if (spriteAtlases.TryGetValue(atlasAddress, out var cachedAtlas))
            {
                Debug.Log($"Sprite Atlas '{atlasAddress}' retrieved from cache.");
                return cachedAtlas;
            }

            var handle = Addressables.LoadAssetAsync<SpriteAtlas>(atlasAddress);
            SpriteAtlas atlas = null;

            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    spriteAtlases[atlasAddress] = operation.Result;
                    atlas = operation.Result;
                    Debug.Log($"Sprite Atlas '{atlasAddress}' loaded successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to load Sprite Atlas with address: {atlasAddress}");
                }
            };

            // Monitor progress
            while (!handle.IsDone)
            {
                // Optionally, implement progress reporting here
                await Task.Yield(); // Wait for the next frame
            }

            return atlas;
        }

        /// <summary>
        /// Unloads a specific Sprite Atlas.
        /// </summary>
        /// <param name="atlasAddress">The address of the Sprite Atlas to unload.</param>
        public void UnloadSpriteAtlas(string atlasAddress)
        {
            if (spriteAtlases.ContainsKey(atlasAddress))
            {
                Addressables.Release(spriteAtlases[atlasAddress]);
                spriteAtlases.Remove(atlasAddress);
                Debug.Log($"Sprite Atlas '{atlasAddress}' unloaded successfully.");
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
            foreach (var key in new List<string>(characterPrefabs.Keys))
            {
                UnloadCharacterPrefab(key);
            }

            // Unload Equipment Prefabs
            foreach (var key in new List<string>(equipmentPrefabs.Keys))
            {
                UnloadEquipmentPrefab(key);
            }

            // Unload Sprite Atlases
            foreach (var key in new List<string>(spriteAtlases.Keys))
            {
                UnloadSpriteAtlas(key);
            }

            Debug.Log("All assets unloaded successfully.");
        }

        #endregion
    }
}