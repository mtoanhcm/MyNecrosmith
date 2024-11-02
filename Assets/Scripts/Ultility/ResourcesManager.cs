using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Character;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace GameUtility
{
    public class ResourcesManager : MonoBehaviour
    {

    // Caches to store loaded assets
    private Dictionary<string, CharacterBase> _characterPrefabs = new Dictionary<string, CharacterBase>();
    private Dictionary<string, SpriteAtlas> _spriteAtlases = new Dictionary<string, SpriteAtlas>();

    // Events for Character Prefab Loading
    public event Action<string, float> OnCharacterPrefabLoadProgress;

    // Events for Sprite Atlas Loading
    public event Action<string, float> OnSpriteAtlasLoadProgress;

    private void OnDestroy()
    {
        UnloadAllAssets();
    }

    public CharacterBase GetCharacterPrefab(string characterClass)
    {
        if (_characterPrefabs.TryGetValue(characterClass, out var cachedPrefab))
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
        var handle = Addressables.LoadAssetAsync<CharacterBase>(characterClass);
        
        // Subscribe to progress updates
        handle.Completed += (operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                _characterPrefabs[characterClass] = operation.Result;
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

        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    /// <summary>
    /// Unloads a specific character prefab.
    /// </summary>
    /// <param name="characterClass">The class of the character to unload.</param>
    public void UnloadCharacterPrefab(string characterClass)
    {
        if (_characterPrefabs.ContainsKey(characterClass))
        {
            Addressables.Release(_characterPrefabs[characterClass]);
            _characterPrefabs.Remove(characterClass);
            Debug.Log($"Character Prefab '{characterClass}' unloaded successfully.");
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
        if (_spriteAtlases.TryGetValue(atlasAddress, out var cachedAtlas))
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
                _spriteAtlases[atlasAddress] = operation.Result;
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
        if (_spriteAtlases.ContainsKey(atlasAddress))
        {
            Addressables.Release(_spriteAtlases[atlasAddress]);
            _spriteAtlases.Remove(atlasAddress);
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
        foreach (var key in new List<string>(_characterPrefabs.Keys))
        {
            UnloadCharacterPrefab(key);
        }

        // Unload Sprite Atlases
        foreach (var key in new List<string>(_spriteAtlases.Keys))
        {
            UnloadSpriteAtlas(key);
        }

        Debug.Log("All assets unloaded successfully.");
    }

    #endregion
    }
}
