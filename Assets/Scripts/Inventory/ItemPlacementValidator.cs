using System.Collections.Generic;
using Equipment;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Validates item placement in an inventory grid.
    /// Ensures items can be placed at specific positions based on various rules.
    /// </summary>
    public class ItemPlacementValidator
    {
        /// <summary>
        /// The grid manager used for validation
        /// </summary>
        private readonly InventoryGridManager gridManager;
        
        /// <summary>
        /// Types of inventory cells that may have specific placement requirements
        /// </summary>
        public enum CellType
        {
            /// <summary>Standard cell with no special restrictions</summary>
            Normal,
            
            /// <summary>Cell that only accepts weapon items</summary>
            WeaponOnly,
            
            /// <summary>Cell that only accepts armor items</summary>
            ArmorOnly,
            
            /// <summary>Cell that only accepts consumable items</summary>
            ConsumableOnly
        }
        
        /// <summary>
        /// Creates a new validator using the specified grid manager
        /// </summary>
        /// <param name="gridManager">The grid manager to use for validation</param>
        public ItemPlacementValidator(InventoryGridManager gridManager)
        {
            this.gridManager = gridManager;
        }
        
        /// <summary>
        /// Checks if an item can be placed in the inventory at the specified position
        /// </summary>
        /// <param name="item">The item to place</param>
        /// <param name="position">The position to place the item</param>
        /// <returns>True if the item can be placed, false otherwise</returns>
        public bool CanPlaceInInventory(InventoryItem item, Vector2Int position)
        {
            if (item == null)
                return false;
                
            // Check if the position is within bounds
            if (!IsWithinBounds(position, item.Width, item.Height, gridManager.Rows, gridManager.Columns))
                return false;
                
            // Check if the item overlaps with other items
            List<string> overlappingItems = gridManager.GetOverlappingItems(item, position);
            if (overlappingItems.Count > 0)
                return false;
                
            // Check if the item meets any special cell type restrictions
            if (!CheckCellTypeRestrictions(item, position))
                return false;
                
            return true;
        }
        
        /// <summary>
        /// Checks if a position and dimensions are within the grid bounds
        /// </summary>
        /// <param name="position">Top-left position</param>
        /// <param name="width">Width in cells</param>
        /// <param name="height">Height in cells</param>
        /// <param name="maxRows">Maximum number of rows</param>
        /// <param name="maxColumns">Maximum number of columns</param>
        /// <returns>True if within bounds, false otherwise</returns>
        public bool IsWithinBounds(Vector2Int position, int width, int height, int maxRows, int maxColumns)
        {
            return position.x >= 0 && position.y >= 0 && 
                   position.x + height <= maxRows && position.y + width <= maxColumns;
        }
        
        /// <summary>
        /// Checks if the item has any overlapping items at the specified position
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="position">The position to check</param>
        /// <returns>True if there are overlapping items, false otherwise</returns>
        public bool HasOverlappingItems(InventoryItem item, Vector2Int position)
        {
            return gridManager.GetOverlappingItems(item, position).Count > 0;
        }
        
        /// <summary>
        /// Checks if the item satisfies all cell type restrictions at the specified position
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="position">The position to check</param>
        /// <returns>True if all cell type restrictions are satisfied, false otherwise</returns>
        private bool CheckCellTypeRestrictions(InventoryItem item, Vector2Int position)
        {
            // In a real implementation, you would check each cell the item would occupy
            // and verify that the item type is compatible with any cell type restrictions
            
            // This is a simplified implementation that always returns true
            return true;
        }
        
        /// <summary>
        /// Checks if an equipment item meets the restrictions for a specific cell type
        /// </summary>
        /// <param name="equipment">The equipment to check</param>
        /// <param name="cellType">The cell type to check against</param>
        /// <returns>True if the equipment can be placed in the cell type, false otherwise</returns>
        public bool MeetsTypeRestrictions(EquipmentData equipment, CellType cellType)
        {
            switch (cellType)
            {
                case CellType.Normal:
                    return true; // All items can go in normal cells
                    
                case CellType.WeaponOnly:
                    return equipment.CategoryID == Config.EquipmentCategoryID.Sword; // Only weapons
                    
                case CellType.ArmorOnly:
                    return equipment.CategoryID == Config.EquipmentCategoryID.Armor; // Only armor
                    
                default:
                    return true;
            }
        }
        
        /// <summary>
        /// Gets a collection of all positions an item would occupy at the specified position
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="position">The top-left position</param>
        /// <returns>Collection of all positions the item would occupy</returns>
        public HashSet<(int, int)> GetOccupiedPositions(InventoryItem item, Vector2Int position)
        {
            HashSet<(int, int)> positions = new HashSet<(int, int)>();
            
            for (int r = 0; r < item.Height; r++)
            {
                for (int c = 0; c < item.Width; c++)
                {
                    positions.Add((position.x + r, position.y + c));
                }
            }
            
            return positions;
        }
    }
}