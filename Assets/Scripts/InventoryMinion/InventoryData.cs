using System.Collections.Generic;
using System.Linq;
using Config;
using Equipment;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Stores the configuration and state of an inventory.
    /// Manages the collection of items and provides methods to manipulate them.
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        /// <summary>
        /// Number of rows in the inventory grid
        /// </summary>
        public int Row { get; private set; }
        
        /// <summary>
        /// Number of columns in the inventory grid
        /// </summary>
        public int Column { get; private set; }
        
        /// <summary>
        /// ID of the character owning this inventory
        /// </summary>
        public CharacterID CharacterID { get; private set; }
        
        /// <summary>
        /// Collection of items contained in this inventory
        /// </summary>
        public List<InventoryItem> Items { get; private set; }
        
        /// <summary>
        /// Creates a new inventory with the specified dimensions
        /// </summary>
        /// <param name="row">Number of rows</param>
        /// <param name="column">Number of columns</param>
        /// <param name="characterID">ID of the owning character</param>
        public InventoryData(int row, int column, CharacterID characterID)
        {
            // Ensure dimensions are within constraints
            Row = Mathf.Clamp(row, InventoryConstants.MIN_ROW, InventoryConstants.MAX_ROW);
            Column = Mathf.Clamp(column, InventoryConstants.MIN_COLUMN, InventoryConstants.MAX_COLUMN);
            
            Items = new List<InventoryItem>();
            CharacterID = characterID;
        }
        
        /// <summary>
        /// Gets an array of all equipment data in this inventory
        /// </summary>
        /// <returns>Array of equipment data</returns>
        public EquipmentData[] GetEquipmentData()
        {
            int totalItem = Items.Count;
            var tempEquipmentLst = new EquipmentData[totalItem];
            
            if (totalItem == 0)
            {
                return tempEquipmentLst;
            }

            for (var i = 0; i < totalItem; i++)
            {
                tempEquipmentLst[i] = Items[i].Equipment;
            }
            
            return tempEquipmentLst;
        }
        
        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">The inventory item to add</param>
        public void AddItem(InventoryItem item)
        {
            if (item == null || Items.Any(i => i.Equipment.EquipmentID == item.Equipment.EquipmentID))
            {
                return; // Don't add null items or duplicates
            }
            
            Items.Add(item);
        }
        
        /// <summary>
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="equipmentID">ID of the equipment to remove</param>
        /// <returns>True if an item was removed, false otherwise</returns>
        public bool RemoveItem(EquipmentID equipmentID)
        {
            int index = Items.FindIndex(item => item.Equipment.EquipmentID == equipmentID);
            if (index >= 0)
            {
                Items.RemoveAt(index);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Serializes this inventory to a string representation
        /// </summary>
        /// <returns>String representation of this inventory</returns>
        public string Serialize()
        {
            // This is a placeholder for actual serialization logic
            return JsonUtility.ToJson(this);
        }
        
        /// <summary>
        /// Deserializes an inventory from a string representation
        /// </summary>
        /// <param name="data">Serialized inventory data</param>
        public void Deserialize(string data)
        {
            // This is a placeholder for actual deserialization logic
            JsonUtility.FromJsonOverwrite(data, this);
        }
    }
}