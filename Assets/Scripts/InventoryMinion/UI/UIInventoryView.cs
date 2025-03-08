using System;
using System.Threading.Tasks;
using Config;
using Equipment;
using UnityEngine;

namespace Inventory.UI
{
    /// <summary>
    /// Manages the main inventory view and its components.
    /// Acts as the coordinator between inventory data and UI components.
    /// </summary>
    public class UIInventoryView : MonoBehaviour
    {
        /// <summary>
        /// The inventory panel component
        /// </summary>
        [SerializeField] private UIInventoryPanel inventoryPanel;
        
        /// <summary>
        /// The equipment tooltip component
        /// </summary>
        [SerializeField] private UIInventoryEquipmentChoosenView equipmentTooltip;
        
        /// <summary>
        /// Current inventory being displayed
        /// </summary>
        private InventoryData currentInventory;
        
        /// <summary>
        /// Event raised when the inventory is opened
        /// </summary>
        public event Action<InventoryData> OnInventoryOpened;
        
        /// <summary>
        /// Event raised when the inventory is closed
        /// </summary>
        public event Action OnInventoryClosed;
        
        /// <summary>
        /// Opens the character inventory
        /// </summary>
        /// <param name="data">The inventory data to display</param>
        public void OpenCharacterInventory(InventoryData data)
        {
            gameObject.SetActive(data != null);
            currentInventory = data;
            
            if (data == null)
            {
                OnInventoryClosed?.Invoke();
                return;
            }
            
            // Open the inventory panel with the specified data
            inventoryPanel.OpenInventory(data);
            
            // Notify listeners
            OnInventoryOpened?.Invoke(data);
        }
        
        /// <summary>
        /// Refreshes the inventory view
        /// </summary>
        public void RefreshInventory()
        {
            if (currentInventory != null)
            {
                inventoryPanel.OpenInventory(currentInventory);
            }
        }
        
        /// <summary>
        /// Closes the inventory
        /// </summary>
        public void CloseInventory()
        {
            gameObject.SetActive(false);
            currentInventory = null;
            OnInventoryClosed?.Invoke();
        }
        
        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="equipmentData">The equipment data to add</param>
        /// <returns>True if the item was added, false otherwise</returns>
        public bool AddItemToInventory(EquipmentData equipmentData)
        {
            if (currentInventory == null || equipmentData == null)
                return false;
                
            // Create a new inventory item
            InventoryItem item = new InventoryItem(equipmentData);
            
            // TODO: Find a suitable position for the item
            
            // Add to the inventory
            currentInventory.AddItem(item);
            
            // Refresh the view
            RefreshInventory();
            
            return true;
        }
        
        /// <summary>
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="equipmentID">The ID of the equipment to remove</param>
        /// <returns>True if the item was removed, false otherwise</returns>
        public bool RemoveItemFromInventory(EquipmentID equipmentID)
        {
            if (currentInventory == null)
                return false;
                
            bool removed = currentInventory.RemoveItem(equipmentID);
            
            if (removed)
            {
                // Refresh the view
                RefreshInventory();
            }
            
            return removed;
        }
        
        /// <summary>
        /// Gets the current inventory
        /// </summary>
        /// <returns>The current inventory, or null if none is open</returns>
        public InventoryData GetCurrentInventory()
        {
            return currentInventory;
        }
        
        /// <summary>
        /// Saves the current inventory state asynchronously
        /// </summary>
        /// <param name="key">Key to use for saving</param>
        /// <returns>True if the save was successful, false otherwise</returns>
        public bool SaveInventory(string key)
        {
            if (currentInventory == null)
                return false;
                
            // Use the InventorySerializer to save the inventory
            InventorySerializer.SaveToPlayerPrefs(key, currentInventory);
            
            return true;
        }
        
        /// <summary>
        /// Loads an inventory state asynchronously
        /// </summary>
        /// <param name="key">Key used for saving</param>
        /// <param name="characterID">ID of the character that owns the inventory</param>
        /// <returns>True if the load was successful, false otherwise</returns>
        public async Task<bool> LoadInventoryAsync(string key, CharacterID characterID)
        {
            // Use the InventorySerializer to load the inventory
            var loadedInventory = await InventorySerializer.LoadFromPlayerPrefsAsync(key, characterID);
            
            if (loadedInventory != null)
            {
                OpenCharacterInventory(loadedInventory);
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Loads an inventory state - this is a compatibility method that works with
        /// the async version internally but returns immediately with a minimal inventory
        /// </summary>
        /// <param name="key">Key used for saving</param>
        /// <param name="characterID">ID of the character that owns the inventory</param>
        /// <returns>True if process started successfully</returns>
        public bool LoadInventory(string key, CharacterID characterID)
        {
            Debug.LogWarning($"Using synchronous LoadInventory. Consider switching to LoadInventoryAsync for better performance.");
            
            // Create a new minimal inventory to use while async loading happens
            var tempInventory = new InventoryData(
                InventoryConstants.MIN_ROW,
                InventoryConstants.MIN_COLUMN,
                characterID
            );
            
            // Show the minimal inventory
            OpenCharacterInventory(tempInventory);
            
            // Start an async task to load the real inventory
            LoadInventoryInBackgroundAndUpdate(key, characterID);
            
            return true;
        }
        
        /// <summary>
        /// Asynchronously loads the inventory in the background and updates the UI when complete
        /// </summary>
        private async void LoadInventoryInBackgroundAndUpdate(string key, CharacterID characterID)
        {
            try
            {
                var loadedInventory = await InventorySerializer.LoadFromPlayerPrefsAsync(key, characterID);
                
                if (loadedInventory != null)
                {
                    // Update the current inventory
                    currentInventory = loadedInventory;
                    
                    // Refresh the UI
                    RefreshInventory();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading inventory in background: {ex.Message}");
            }
        }
    }
}