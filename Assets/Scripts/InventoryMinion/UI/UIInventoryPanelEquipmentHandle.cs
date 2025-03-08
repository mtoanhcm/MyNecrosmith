using System.Collections.Generic;
using Config;
using Equipment;

namespace Inventory.UI
{
    /// <summary>
    /// Handles operations on inventory equipment items.
    /// Manages the collection of equipment items in the inventory.
    /// </summary>
    public class UIInventoryPanelEquipmentHandle
    {
        /// <summary>
        /// List of inventory items in the panel
        /// </summary>
        private List<UIInventoryItem> inventoryItems;

        /// <summary>
        /// Creates a new equipment handle
        /// </summary>
        public UIInventoryPanelEquipmentHandle()
        {
            inventoryItems = new List<UIInventoryItem>();
        }

        /// <summary>
        /// Sets the list of inventory items
        /// </summary>
        /// <param name="inventoryItems">The new list of items, or null to clear</param>
        public void SetInventoryItems(List<UIInventoryItem> inventoryItems)
        {
            this.inventoryItems.Clear();

            if (inventoryItems != null)
            {
                this.inventoryItems = inventoryItems;   
            }
        }

        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddItemToInventory(UIInventoryItem item)
        {
            if (item == null)
                return;
                
            // Avoid duplicates
            if (!inventoryItems.Contains(item))
            {
                inventoryItems.Add(item);
            }
        }
        
        /// <summary>
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if the item was removed, false otherwise</returns>
        public bool RemoveItemFromInventory(UIInventoryItem item)
        {
            if (item == null)
                return false;
                
            return inventoryItems.Remove(item);
        }
        
        /// <summary>
        /// Gets the item with the specified ID
        /// </summary>
        /// <param name="equipmentID">The equipment ID to find</param>
        /// <returns>The matching item, or null if not found</returns>
        public UIInventoryItem GetItemByID(EquipmentID equipmentID)
        {
            return inventoryItems.Find(item => item.Item.Equipment.EquipmentID == equipmentID);
        }

        /// <summary>
        /// Gets a list of all equipment data in the inventory
        /// </summary>
        /// <returns>List of equipment data</returns>
        public List<EquipmentData> GetEquipmentData()
        {
            var equipmentData = new List<EquipmentData>();
            
            // Extract equipment data from each inventory item
            for (var i = 0; i < inventoryItems.Count; i++)
            {
                equipmentData.Add(inventoryItems[i].Item.Equipment);
            }
            
            return equipmentData;
        }
        
        /// <summary>
        /// Gets the number of items in the inventory
        /// </summary>
        public int ItemCount => inventoryItems.Count;
        
        /// <summary>
        /// Gets the total load of all items in the inventory
        /// </summary>
        public int TotalLoad
        {
            get
            {
                int total = 0;
                foreach (var item in inventoryItems)
                {
                    total += item.Item.Equipment.LoadPoint;
                }
                return total;
            }
        }
    }
}