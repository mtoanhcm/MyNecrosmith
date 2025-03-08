using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config;
using Equipment;
using Inventory.UI;
using Observer;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Core manager for the inventory system.
    /// Coordinates between data layer, logic layer, and UI layer.
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        /// <summary>
        /// The UI view for the inventory
        /// </summary>
        [SerializeField] private UIInventoryView uiView;
        
        /// <summary>
        /// The grid manager for the inventory
        /// </summary>
        private InventoryGridManager gridManager;
        
        /// <summary>
        /// The placement validator for items
        /// </summary>
        private ItemPlacementValidator placementValidator;
        
        /// <summary>
        /// The currently active inventory
        /// </summary>
        private InventoryData currentInventory;
        
        /// <summary>
        /// Event raised when an item is added to the inventory
        /// </summary>
        public event Action<EquipmentData> OnItemAdded;
        
        /// <summary>
        /// Event raised when an item is removed from the inventory
        /// </summary>
        public event Action<EquipmentID> OnItemRemoved;
        
        /// <summary>
        /// Event raised when an item is moved in the inventory
        /// </summary>
        public event Action<InventoryItem, Vector2Int> OnItemMoved;
        
        /// <summary>
        /// Initializes the inventory system
        /// </summary>
        private void Awake()
        {
            // Register for events
            EventManager.Instance.StartListening<EventData.OpenCharacterInventory>(OnOpenCharacterInventory);
            EventManager.Instance.StartListening<EventData.OnChooseEquipmentInStorage>(OnChooseEquipmentInStorage);
        }
        
        /// <summary>
        /// Cleans up the inventory system
        /// </summary>
        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OpenCharacterInventory>(OnOpenCharacterInventory);
            EventManager.Instance?.StopListening<EventData.OnChooseEquipmentInStorage>(OnChooseEquipmentInStorage);
        }
        
        /// <summary>
        /// Initializes the inventory for a character
        /// </summary>
        /// <param name="characterID">ID of the character</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        public void Initialize(CharacterID characterID, int rows, int columns)
        {
            // Create the inventory data
            currentInventory = new InventoryData(rows, columns, characterID);
            
            // Create the grid manager
            gridManager = new InventoryGridManager(rows, columns);
            
            // Create the placement validator
            placementValidator = new ItemPlacementValidator(gridManager);
            
            // Initialize the UI
            if (uiView != null)
            {
                uiView.OpenCharacterInventory(currentInventory);
            }
        }
        
        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="equipment">The equipment to add</param>
        /// <param name="position">Optional position, or null for automatic placement</param>
        /// <returns>True if the item was added, false otherwise</returns>
        public bool AddItem(EquipmentData equipment, Vector2Int? position = null)
        {
            if (currentInventory == null || equipment == null)
                return false;
                
            // Create the inventory item
            InventoryItem item = new InventoryItem(equipment);
            
            // Find a position if none was specified
            if (position == null)
            {
                position = gridManager.FindEmptySpaceFor(item);
                
                if (position == null)
                    return false; // No space available
            }
            
            // Check if the position is valid
            if (!placementValidator.CanPlaceInInventory(item, position.Value))
                return false;
                
            // Get the positions the item will occupy
            HashSet<(int, int)> occupiedPositions = placementValidator.GetOccupiedPositions(item, position.Value);
            
            // Update the item's position
            item.UpdatePosInInventory(occupiedPositions);
            
            // Add to the inventory
            currentInventory.AddItem(item);
            
            // Update the grid
            gridManager.UpdateGridState(currentInventory.Items);
            
            // Notify listeners
            OnItemAdded?.Invoke(equipment);
            
            // Refresh the UI
            if (uiView != null)
            {
                uiView.RefreshInventory();
            }
            
            return true;
        }
        
        /// <summary>
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="equipmentID">ID of the equipment to remove</param>
        /// <returns>True if the item was removed, false otherwise</returns>
        public bool RemoveItem(EquipmentID equipmentID)
        {
            if (currentInventory == null)
                return false;
                
            bool removed = currentInventory.RemoveItem(equipmentID);
            
            if (removed)
            {
                // Update the grid
                gridManager.UpdateGridState(currentInventory.Items);
                
                // Notify listeners
                OnItemRemoved?.Invoke(equipmentID);
                
                // Refresh the UI
                if (uiView != null)
                {
                    uiView.RefreshInventory();
                }
            }
            
            return removed;
        }
        
        /// <summary>
        /// Moves an item to a new position
        /// </summary>
        /// <param name="item">The item to move</param>
        /// <param name="newPosition">The new position</param>
        /// <returns>True if the item was moved, false otherwise</returns>
        public bool MoveItem(InventoryItem item, Vector2Int newPosition)
        {
            if (currentInventory == null || item == null)
                return false;
                
            // Check if the position is valid
            if (!placementValidator.CanPlaceInInventory(item, newPosition))
                return false;
                
            // Get the positions the item will occupy
            HashSet<(int, int)> occupiedPositions = placementValidator.GetOccupiedPositions(item, newPosition);
            
            // Update the item's position
            item.UpdatePosInInventory(occupiedPositions);
            
            // Update the grid
            gridManager.UpdateGridState(currentInventory.Items);
            
            // Notify listeners
            OnItemMoved?.Invoke(item, newPosition);
            
            // Refresh the UI
            if (uiView != null)
            {
                uiView.RefreshInventory();
            }
            
            return true;
        }
        
        /// <summary>
        /// Gets an item by its equipment ID
        /// </summary>
        /// <param name="equipmentID">The equipment ID to find</param>
        /// <returns>The matching item, or null if not found</returns>
        public InventoryItem GetItem(EquipmentID equipmentID)
        {
            if (currentInventory == null)
                return null;
                
            return currentInventory.Items.Find(item => item.Equipment.EquipmentID == equipmentID);
        }
        
        /// <summary>
        /// Shows the inventory UI
        /// </summary>
        public void Show()
        {
            if (uiView != null)
            {
                uiView.gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        /// Hides the inventory UI
        /// </summary>
        public void Hide()
        {
            if (uiView != null)
            {
                uiView.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Gets the current inventory
        /// </summary>
        /// <returns>The current inventory, or null if none is active</returns>
        public InventoryData GetCurrentInventory()
        {
            return currentInventory;
        }
        
        /// <summary>
        /// Saves the current inventory state
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
            // Use the InventorySerializer to load the inventory asynchronously
            var loadedInventory = await InventorySerializer.LoadFromPlayerPrefsAsync(key, characterID);
            
            if (loadedInventory != null)
            {
                currentInventory = loadedInventory;
                
                // Create or update the grid manager
                if (gridManager == null)
                {
                    gridManager = new InventoryGridManager(loadedInventory.Row, loadedInventory.Column);
                }
                
                // Create or update the placement validator
                if (placementValidator == null)
                {
                    placementValidator = new ItemPlacementValidator(gridManager);
                }
                
                // Update the grid
                gridManager.UpdateGridState(currentInventory.Items);
                
                // Refresh the UI
                if (uiView != null)
                {
                    uiView.OpenCharacterInventory(currentInventory);
                }
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Handles the OpenCharacterInventory event
        /// </summary>
        /// <param name="data">Event data</param>
        private void OnOpenCharacterInventory(EventData.OpenCharacterInventory data)
        {
            if (data.InventoryData == null)
            {
                Hide();
                return;
            }
            
            // Create a new inventory for the character
            Initialize(data.InventoryData.CharacterID, data.InventoryData.Row, data.InventoryData.Column);
        }
        
        /// <summary>
        /// Handles the OnChooseEquipmentInStorage event
        /// </summary>
        /// <param name="data">Event data</param>
        private void OnChooseEquipmentInStorage(EventData.OnChooseEquipmentInStorage data)
        {
            // Equipment selection is handled by the UI components
        }
        
        /// <summary>
        /// For backwards compatibility, provides a synchronous version of LoadInventory.
        /// This method creates a dummy inventory if needed, rather than trying to load with Resources.Load
        /// </summary>
        /// <param name="key">Key used for saving</param>
        /// <param name="characterID">ID of the character that owns the inventory</param>
        /// <returns>True if the load was successful, false otherwise</returns>
        public bool LoadInventory(string key, CharacterID characterID)
        {
            Debug.LogWarning("Using synchronous LoadInventory. Consider switching to LoadInventoryAsync for better performance.");
            
            // Create a new inventory 
            var newInventory = new InventoryData(
                InventoryConstants.MIN_ROW,
                InventoryConstants.MIN_COLUMN,
                characterID
            );
            
            currentInventory = newInventory;
            
            // Create or update the grid manager
            if (gridManager == null)
            {
                gridManager = new InventoryGridManager(newInventory.Row, newInventory.Column);
            }
            
            // Create or update the placement validator
            if (placementValidator == null)
            {
                placementValidator = new ItemPlacementValidator(gridManager);
            }
            
            // Update the grid
            gridManager.UpdateGridState(currentInventory.Items);
            
            // Refresh the UI
            if (uiView != null)
            {
                uiView.OpenCharacterInventory(currentInventory);
            }
            
            // Start async load in background to update with real data when it completes
            LoadInventoryInBackground(key, characterID);
            
            return true;
        }
        
        /// <summary>
        /// Loads inventory data in the background and updates when complete
        /// </summary>
        private async void LoadInventoryInBackground(string key, CharacterID characterID)
        {
            var loadedInventory = await InventorySerializer.LoadFromPlayerPrefsAsync(key, characterID);
            
            if (loadedInventory != null)
            {
                currentInventory = loadedInventory;
                
                // Update the grid
                gridManager.UpdateGridState(currentInventory.Items);
                
                // Refresh the UI
                if (uiView != null)
                {
                    uiView.RefreshInventory();
                }
            }
        }
    }
}