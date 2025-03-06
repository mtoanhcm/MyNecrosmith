using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Static class that defines all constants used by the inventory system.
    /// These values determine the limits and dimensions for all inventory functionality.
    /// </summary>
    public static class InventoryConstants
    {
        // Minimum and maximum dimensions for inventory grids
        public const int MIN_ROW = 2;
        public const int MIN_COLUMN = 2;
        public const int MAX_ROW = 7;
        public const int MAX_COLUMN = 9;
        
        // UI dimensions in pixels
        public const int CELL_SIZE = 50;
        public const int CELL_SPACING = 5;
        
        // Maximum dimensions for equipment items
        public const int MAX_EQUIPMENT_WIDTH = 4;
        public const int MAX_EQUIPMENT_HEIGHT = 4;
    }
}