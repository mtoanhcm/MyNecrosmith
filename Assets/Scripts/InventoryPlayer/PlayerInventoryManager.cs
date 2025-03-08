using System;
using System.Collections.Generic;
using Config;
using Equipment;
using Inventory;
using Observer;
using UnityEngine;

namespace Player.Inventory
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public static PlayerInventoryManager Instance { get; private set; }
        
        // The player's inventory data
        private PlayerInventoryData inventoryData;
        
        // Delegate for inventory change events
        public delegate void InventoryChangedHandler(EquipmentCategoryID category);
        
        // Events
        public event InventoryChangedHandler OnInventoryChanged;
        public event Action<EquipmentData> OnEquipmentAdded;
        public event Action<EquipmentData> OnEquipmentRemoved;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                
                // Initialize inventory if needed
                int maxSlots = InventoryConstants.PLAYER_INVENTORY_MAX_ROW * InventoryConstants.PLAYER_INVENTORY_MAX_COLUMN;
                inventoryData = new PlayerInventoryData(maxSlots);
            }
        }
        
        private void Start()
        {
            // Subscribe to events
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
            EventManager.Instance.StartListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipmentFromPlayerStorage);
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                // Unsubscribe from events
                EventManager.Instance?.StopListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
                EventManager.Instance?.StopListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipmentFromPlayerStorage);
            }
        }
        
        /// <summary>
        /// Adds equipment to the player's inventory
        /// </summary>
        /// <param name="equipment">The equipment to add</param>
        /// <returns>True if successfully added</returns>
        public bool AddEquipment(EquipmentData equipment)
        {
            if (equipment == null)
                return false;
                
            bool success = inventoryData.AddEquipment(equipment);
            
            if (success)
            {
                // Notify listeners
                OnInventoryChanged?.Invoke(equipment.CategoryID);
                OnEquipmentAdded?.Invoke(equipment);
            }
            
            return success;
        }
        
        /// <summary>
        /// Removes equipment from the player's inventory
        /// </summary>
        /// <param name="equipment">The equipment to remove</param>
        /// <param name="removeAll">Whether to remove all stacks</param>
        /// <returns>True if successfully removed</returns>
        public bool RemoveEquipment(EquipmentData equipment, bool removeAll = false)
        {
            if (equipment == null)
                return false;
                
            bool success = inventoryData.RemoveEquipment(equipment, removeAll);
            
            if (success)
            {
                // Notify listeners
                OnInventoryChanged?.Invoke(equipment.CategoryID);
                OnEquipmentRemoved?.Invoke(equipment);
            }
            
            return success;
        }
        
        /// <summary>
        /// Gets all equipment slots for a category and page
        /// </summary>
        /// <param name="category">The category to get slots for</param>
        /// <param name="page">The page index (0-based)</param>
        /// <returns>List of equipment slots</returns>
        public List<PlayerInventoryData.EquipmentSlot> GetEquipmentSlots(EquipmentCategoryID category, int page)
        {
            return inventoryData.GetEquipmentSlotsForPage(category, page);
        }
        
        /// <summary>
        /// Gets the total number of pages for a category
        /// </summary>
        /// <param name="category">The category to check</param>
        /// <returns>Number of pages</returns>
        public int GetPageCount(EquipmentCategoryID category)
        {
            return inventoryData.GetPageCount(category);
        }
        
        /// <summary>
        /// Gets all categories that have at least one item
        /// </summary>
        /// <returns>List of populated categories</returns>
        public List<EquipmentCategoryID> GetPopulatedCategories()
        {
            return inventoryData.GetPopulatedCategories();
        }
        
        /// <summary>
        /// Checks if a category has any items
        /// </summary>
        /// <param name="category">The category to check</param>
        /// <returns>True if the category has items</returns>
        public bool HasItems(EquipmentCategoryID category)
        {
            return inventoryData.HasItems(category);
        }
        
        /// <summary>
        /// Event handler for OnObtainedEquipment
        /// </summary>
        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            AddEquipment(data.EquipmentData);
        }
        
        /// <summary>
        /// Event handler for OnRemoveEquipmentFromPlayerStorage
        /// </summary>
        private void OnRemoveEquipmentFromPlayerStorage(EventData.OnRemoveEquipmentFromPlayerStorage data)
        {
            // Find the equipment with the matching ID
            foreach (var category in GetPopulatedCategories())
            {
                foreach (var slot in inventoryData.GetEquipmentSlots(category))
                {
                    if (slot.Equipment.EquipmentID == data.EquipmentID)
                    {
                        RemoveEquipment(slot.Equipment);
                        return;
                    }
                }
            }
        }
    }
}