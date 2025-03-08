using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Equipment;
using UnityEngine;

namespace Player.Inventory
{
    [Serializable]
    public class PlayerInventoryData
    {
        [Serializable]
        public class EquipmentSlot
        {
            public EquipmentData Equipment;
            public int StackCount;
            public bool IsLocked;

            public EquipmentSlot(EquipmentData equipment, int stackCount = 1)
            {
                Equipment = equipment;
                StackCount = stackCount;
                IsLocked = false;
            }

            public void AddStack()
            {
                StackCount++;
            }

            public bool RemoveStack()
            {
                StackCount--;
                return StackCount <= 0;
            }

            public bool CanStackWith(EquipmentData otherEquipment)
            {
                if (Equipment == null || otherEquipment == null)
                    return false;

                // Stack if same ID, level and rarity
                return Equipment.EquipmentID == otherEquipment.EquipmentID;
                
                // return Equipment.EquipmentID == otherEquipment.EquipmentID && 
                //        Equipment.GetLevel() == otherEquipment.GetLevel() &&
                //        Equipment.GetRarity() == otherEquipment.GetRarity();
            }
        }

        // All equipment slots organized by category
        private Dictionary<EquipmentCategoryID, List<EquipmentSlot>> equipmentSlots;
        
        // Maximum number of slots per page
        private int maxSlotsPerPage;

        public PlayerInventoryData(int maxSlotsPerPage = 24)
        {
            this.maxSlotsPerPage = maxSlotsPerPage;
            equipmentSlots = new Dictionary<EquipmentCategoryID, List<EquipmentSlot>>();
            
            // Initialize all equipment categories
            foreach (EquipmentCategoryID categoryID in Enum.GetValues(typeof(EquipmentCategoryID)))
            {
                if (categoryID == EquipmentCategoryID.None)
                {
                    continue;
                }
                
                equipmentSlots[categoryID] = new List<EquipmentSlot>();
            }
            
            Debug.Log("Finish init player inventory");
        }

        /// <summary>
        /// Adds equipment to the inventory
        /// </summary>
        /// <param name="equipment">The equipment to add</param>
        /// <returns>True if successfully added</returns>
        public bool AddEquipment(EquipmentData equipment)
        {
            if (equipment == null)
                return false;

            var category = equipment.CategoryID;
            
            // Try to stack with existing equipment
            foreach (var slot in equipmentSlots[category])
            {
                if (slot.CanStackWith(equipment))
                {
                    slot.AddStack();
                    return true;
                }
            }

            // If we can't stack, add as new slot
            equipmentSlots[category].Add(new EquipmentSlot(equipment));
            return true;
        }

        /// <summary>
        /// Removes equipment from inventory
        /// </summary>
        /// <param name="equipment">The equipment to remove</param>
        /// <param name="removeAll">Whether to remove all stacks</param>
        /// <returns>True if successfully removed</returns>
        public bool RemoveEquipment(EquipmentData equipment, bool removeAll = false)
        {
            if (equipment == null)
                return false;

            var category = equipment.CategoryID;
            
            for (int i = 0; i < equipmentSlots[category].Count; i++)
            {
                var slot = equipmentSlots[category][i];
                
                if (slot.CanStackWith(equipment))
                {
                    if (removeAll || slot.StackCount <= 1)
                    {
                        equipmentSlots[category].RemoveAt(i);
                    }
                    else
                    {
                        slot.RemoveStack();
                    }
                    
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Gets all equipment slots for a category
        /// </summary>
        /// <param name="category">The category to get slots for</param>
        /// <returns>List of equipment slots</returns>
        public List<EquipmentSlot> GetEquipmentSlots(EquipmentCategoryID category)
        {
            if (equipmentSlots.TryGetValue(category, out var slots))
            {
                return slots;
            }
            
            return new List<EquipmentSlot>();
        }

        /// <summary>
        /// Gets slots for a specific page of a category
        /// </summary>
        /// <param name="category">The category to get slots for</param>
        /// <param name="page">The page index (0-based)</param>
        /// <returns>List of equipment slots for the page</returns>
        public List<EquipmentSlot> GetEquipmentSlotsForPage(EquipmentCategoryID category, int page)
        {
            if (equipmentSlots == null || !equipmentSlots.TryGetValue(category, out var allSlots))
            {
                return new List<EquipmentSlot>();
            }
            
            int startIndex = page * maxSlotsPerPage;
            int count = Math.Min(maxSlotsPerPage, allSlots.Count - startIndex);
            
            if (startIndex >= allSlots.Count || count <= 0)
            {
                return new List<EquipmentSlot>();
            }
            
            return allSlots.GetRange(startIndex, count);
        }

        /// <summary>
        /// Gets the total number of pages for a category
        /// </summary>
        /// <param name="category">The category to check</param>
        /// <returns>Number of pages</returns>
        public int GetPageCount(EquipmentCategoryID category)
        {
            if (equipmentSlots == null || !equipmentSlots.TryGetValue(category, out var slots))
            {
                return 0;
            }
            
            return Mathf.CeilToInt((float)slots.Count / maxSlotsPerPage);
        }

        /// <summary>
        /// Gets the total number of items in the inventory
        /// </summary>
        /// <returns>Total item count across all categories</returns>
        public int GetTotalItemCount()
        {
            int count = 0;
            foreach (var categorySlots in equipmentSlots.Values)
            {
                count += categorySlots.Count;
            }
            return count;
        }

        /// <summary>
        /// Gets all equipment categories that have at least one item
        /// </summary>
        /// <returns>List of categories with items</returns>
        public List<EquipmentCategoryID> GetPopulatedCategories()
        {
            if (equipmentSlots == null || equipmentSlots.Count == 0)
            {
                return new List<EquipmentCategoryID>();
            }
            
            return equipmentSlots
                .Where(kvp => kvp.Value.Count > 0)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        /// <summary>
        /// Checks if a category has any items
        /// </summary>
        /// <param name="category">The category to check</param>
        /// <returns>True if the category has items</returns>
        public bool HasItems(EquipmentCategoryID category)
        {
            if (equipmentSlots == null || equipmentSlots.Count == 0)
            {
                return false;
            }
            
            return equipmentSlots.TryGetValue(category, out var slots) && slots.Count > 0;
        }
    }
}