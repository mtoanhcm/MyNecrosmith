using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Config;

namespace Inventory
{
    /// <summary>
    /// Handles serialization and deserialization of inventory data.
    /// Provides methods to save and load inventory state using Addressables.
    /// </summary>
    public static class InventorySerializer
    {
        // Cache for equipment config handles to avoid reloading same configs
        private static Dictionary<string, AsyncOperationHandle<EquipmentConfig>> configHandles = 
            new Dictionary<string, AsyncOperationHandle<EquipmentConfig>>();
        
        /// <summary>
        /// Serializes an inventory to a JSON string
        /// </summary>
        /// <param name="inventory">The inventory to serialize</param>
        /// <returns>JSON representation of the inventory</returns>
        public static string SerializeInventory(InventoryData inventory)
        {
            if (inventory == null)
                return string.Empty;
            
            // Create a serializable representation
            SerializableInventory serializableInventory = new SerializableInventory
            {
                Rows = inventory.Row,
                Columns = inventory.Column,
                CharacterID = (int)inventory.CharacterID,
                Items = new List<SerializableItem>()
            };
            
            // Convert inventory items to serializable format
            foreach (var item in inventory.Items)
            {
                SerializableItem serializableItem = new SerializableItem
                {
                    EquipmentID = (int)item.Equipment.EquipmentID,
                    CategoryID = (int)item.Equipment.CategoryID,
                    Positions = new List<SerializablePosition>()
                };
                
                // Add positions
                foreach (var pos in item.PosClaimInventory)
                {
                    serializableItem.Positions.Add(new SerializablePosition
                    {
                        Row = pos.Item1,
                        Column = pos.Item2
                    });
                }
                
                serializableInventory.Items.Add(serializableItem);
            }
            
            // Convert to JSON
            return JsonUtility.ToJson(serializableInventory);
        }
        
        /// <summary>
        /// Deserializes an inventory from a JSON string asynchronously
        /// </summary>
        /// <param name="data">JSON representation of the inventory</param>
        /// <param name="ownerId">ID of the character that owns the inventory</param>
        /// <returns>Deserialized inventory, or null if deserialization failed</returns>
        public static async Task<InventoryData> DeserializeInventoryAsync(string data, CharacterID ownerId)
        {
            if (string.IsNullOrEmpty(data))
                return null;
                
            try
            {
                // Parse the JSON data
                SerializableInventory serializableInventory = JsonUtility.FromJson<SerializableInventory>(data);
                
                // Create a new inventory
                InventoryData inventory = new InventoryData(
                    serializableInventory.Rows,
                    serializableInventory.Columns,
                    (CharacterID)serializableInventory.CharacterID
                );
                
                // Add items to the inventory
                foreach (var serializableItem in serializableInventory.Items)
                {
                    // Load the equipment data asynchronously
                    EquipmentData equipmentData = await LoadEquipmentDataAsync(
                        (EquipmentID)serializableItem.EquipmentID,
                        (EquipmentCategoryID)serializableItem.CategoryID
                    );
                    
                    if (equipmentData != null)
                    {
                        // Create the inventory item
                        InventoryItem item = new InventoryItem(equipmentData);
                        
                        // Add positions
                        HashSet<(int, int)> positions = new HashSet<(int, int)>();
                        foreach (var pos in serializableItem.Positions)
                        {
                            positions.Add((pos.Row, pos.Column));
                        }
                        
                        item.UpdatePosInInventory(positions);
                        
                        // Add to inventory
                        inventory.AddItem(item);
                    }
                }
                
                return inventory;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to deserialize inventory: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Saves an inventory to PlayerPrefs
        /// </summary>
        /// <param name="key">Key to use for saving in PlayerPrefs</param>
        /// <param name="inventory">The inventory to save</param>
        public static void SaveToPlayerPrefs(string key, InventoryData inventory)
        {
            if (inventory == null)
                return;
                
            string data = SerializeInventory(inventory);
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Loads an inventory from PlayerPrefs asynchronously
        /// </summary>
        /// <param name="key">Key used to save the inventory</param>
        /// <param name="ownerId">ID of the character that owns the inventory</param>
        /// <returns>Loaded inventory, or null if not found</returns>
        public static async Task<InventoryData> LoadFromPlayerPrefsAsync(string key, CharacterID ownerId)
        {
            if (!PlayerPrefs.HasKey(key))
                return null;
                
            string data = PlayerPrefs.GetString(key);
            return await DeserializeInventoryAsync(data, ownerId);
        }
        
        /// <summary>
        /// Saves an inventory to a file
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="inventory">The inventory to save</param>
        public static void SaveToFile(string filePath, InventoryData inventory)
        {
            if (inventory == null)
                return;
                
            string data = SerializeInventory(inventory);
            File.WriteAllText(filePath, data);
        }
        
        /// <summary>
        /// Loads an inventory from a file asynchronously
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="ownerId">ID of the character that owns the inventory</param>
        /// <returns>Loaded inventory, or null if file not found</returns>
        public static async Task<InventoryData> LoadFromFileAsync(string filePath, CharacterID ownerId)
        {
            if (!File.Exists(filePath))
                return null;
                
            string data = File.ReadAllText(filePath);
            return await DeserializeInventoryAsync(data, ownerId);
        }
        
        /// <summary>
        /// Loads equipment data using Addressables
        /// </summary>
        /// <param name="equipmentId">ID of the equipment</param>
        /// <param name="categoryId">Category of the equipment</param>
        /// <returns>Loaded equipment data, or null if not found</returns>
        private static async Task<EquipmentData> LoadEquipmentDataAsync(EquipmentID equipmentId, EquipmentCategoryID categoryId)
        {
            try
            {
                string cacheKey = $"{categoryId}_{equipmentId}";
                
                // Check if we already have a valid handle in the cache
                if (configHandles.TryGetValue(cacheKey, out var cachedHandle) && cachedHandle.IsValid())
                {
                    return CreateEquipmentDataFromConfig(cachedHandle.Result, categoryId);
                }
                
                // Load the equipment config using Addressables
                string address = $"Config/Equipment/{categoryId}/{equipmentId}";
                var handle = Addressables.LoadAssetAsync<EquipmentConfig>(address);
                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Failed to load equipment config: {address}");
                    return null;
                }

                var config = handle.Result;
                
                // Cache the handle for future use
                configHandles[cacheKey] = handle;
                
                if (config == null)
                    return null;
                    
                return CreateEquipmentDataFromConfig(config, categoryId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load equipment data: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Creates appropriate equipment data based on the equipment category
        /// </summary>
        /// <param name="config">Equipment configuration</param>
        /// <param name="categoryId">Category of the equipment</param>
        /// <returns>Equipment data of appropriate type</returns>
        private static EquipmentData CreateEquipmentDataFromConfig(EquipmentConfig config, EquipmentCategoryID categoryId)
        {
            if (config == null)
                return null;
                
            // Create the appropriate equipment data based on category
            if (categoryId == EquipmentCategoryID.Sword)
            {
                return new WeaponData(config);
            }
            else if (categoryId == EquipmentCategoryID.Armor)
            {
                return new ArmorData(config as ArmorConfig);
            }
            
            // Default case - create generic equipment data
            return new EquipmentData(config);
        }
        
        /// <summary>
        /// Releases all cached handles to prevent memory leaks
        /// </summary>
        public static void ReleaseAllHandles()
        {
            foreach (var handle in configHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            configHandles.Clear();
        }
        
        #region Serializable Classes
        
        [Serializable]
        private class SerializableInventory
        {
            public int Rows;
            public int Columns;
            public int CharacterID;
            public List<SerializableItem> Items;
        }
        
        [Serializable]
        private class SerializableItem
        {
            public int EquipmentID;
            public int CategoryID;
            public List<SerializablePosition> Positions;
        }
        
        [Serializable]
        private class SerializablePosition
        {
            public int Row;
            public int Column;
        }
        
        #endregion
    }
}