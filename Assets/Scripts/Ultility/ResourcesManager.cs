using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Character;
using Equipment;
using Observer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace Gameplay
{
    public class ResourcesManager
    {
        // Caches to store loaded assets
        private Dictionary<string, CharacterBase> characterPrefabs = new Dictionary<string, CharacterBase>();
        private Dictionary<string, EquipmentBase> equipmentPrefabs = new Dictionary<string, EquipmentBase>();
        private Dictionary<string, SpriteAtlas> spriteAtlases = new Dictionary<string, SpriteAtlas>();

        // Events for Character Prefab Loading
        public event Action<string, float> OnCharacterPrefabLoadProgress;
        public event Action<string, float> OnEquipmentPrefabLoadProgress;

        // Events for Sprite Atlas Loading
        public event Action<string, float> OnSpriteAtlasLoadProgress;

        public CharacterBase GetCharacterPrefab(string characterClass)
        {
            if (characterPrefabs.TryGetValue(characterClass, out var cachedPrefab))
            {
                Debug.Log($"Character Prefab '{characterClass}' retrieved from cache.");
                return cachedPrefab;
            }

            return null;
        }

        #region Character Prefab Loading

        /// <summary>
        /// Loads a character prefab based on the specified class.
        /// </summary>
        /// <param name="characterClass">The class of the character to load.</param>
        /// <returns>The loaded character prefab.</returns>
        public async Task<CharacterBase> LoadCharacterPrefabAsync(string characterClass)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>($"Character/{characterClass}.prefab");
            CharacterBase characterPrefab = null;
            // Subscribe to progress updates
            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded && operation.Result.TryGetComponent(out characterPrefab))
                {
                    characterPrefabs[characterClass] = characterPrefab;
                    EventManager.Instance.TriggerEvent(new EventData.OnLoadCharacterPrefabSuccess()
                    {
                        Class = characterClass,
                        CharPrefab = characterPrefab 
                    });
                    Debug.Log($"Character Prefab '{characterClass}' loaded successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to load Character Prefab with address: {characterClass}");
                }
            };

            // Monitor progress
            while (!handle.IsDone)
            {
                OnCharacterPrefabLoadProgress?.Invoke(characterClass, handle.PercentComplete);
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
                Addressables.Release(characterPrefabs[characterClass]);
                characterPrefabs.Remove(characterClass);
                Debug.Log($"Character Prefab '{characterClass}' unloaded successfully.");
            }
        }

        #endregion

        #region Equipment Prefab Loading

        /// <summary>
        /// Loads a equipment prefab based on the specified class.
        /// </summary>
        /// <param name="equipmentType">The type of the equipment to load.</param>
        /// <returns>The loaded equipment prefab.</returns>
        public async Task<EquipmentBase> LoadEquipmentPrefabAsync(string equipmentType)
        {
            var handle = Addressables.LoadAssetAsync<EquipmentBase>(equipmentType);

            // Subscribe to progress updates
            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    equipmentPrefabs[equipmentType] = operation.Result;
                    Debug.Log($"Character Prefab '{equipmentType}' loaded successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to load Character Prefab with address: {equipmentType}");
                }
            };

            // Monitor progress
            while (!handle.IsDone)
            {
                OnEquipmentPrefabLoadProgress?.Invoke(equipmentType, handle.PercentComplete);
                await Task.Yield(); // Wait for the next frame
            }

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        /// <summary>
        /// Unloads a specific equipment prefab.
        /// </summary>
        /// <param name="equipmentType">The type of the equipment to unload.</param>
        public void UnloadEquipmentPrefab(string equipmentType)
        {
            if (equipmentPrefabs.ContainsKey(equipmentType))
            {
                Addressables.Release(equipmentPrefabs[equipmentType]);
                equipmentPrefabs.Remove(equipmentType);
                Debug.Log($"Character Prefab '{equipmentType}' unloaded successfully.");
            }
        }

        #endregion

        #region Sprite Atlas Loading

        /// <summary>
        /// Loads a Sprite Atlas based on the specified address.
        /// </summary>
        /// <param name="atlasAddress">The address of the Sprite Atlas to load.</param>
        /// <returns>The loaded SpriteAtlas.</returns>
        public async Task<SpriteAtlas> LoadSpriteAtlasAsync(string atlasAddress)
        {
            if (spriteAtlases.TryGetValue(atlasAddress, out var cachedAtlas))
            {
                Debug.Log($"Sprite Atlas '{atlasAddress}' retrieved from cache.");
                return cachedAtlas;
            }

            var handle = Addressables.LoadAssetAsync<SpriteAtlas>(atlasAddress);

            // Subscribe to progress updates
            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    spriteAtlases[atlasAddress] = operation.Result;
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
                OnSpriteAtlasLoadProgress?.Invoke(atlasAddress, handle.PercentComplete);
                await Task.Yield(); // Wait for the next frame
            }

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
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
