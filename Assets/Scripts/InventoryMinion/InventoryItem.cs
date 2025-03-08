using System.Collections.Generic;
using Equipment;

namespace Inventory
{
    /// <summary>
    /// Represents an item placed in an inventory with position information.
    /// Acts as a wrapper around EquipmentData while tracking its position in the inventory grid.
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// The equipment data this inventory item represents
        /// </summary>
        public EquipmentData Equipment { get; private set; }
        
        /// <summary>
        /// The set of positions this item occupies in the inventory grid
        /// </summary>
        public HashSet<(int, int)> PosClaimInventory { get; private set; }
        
        /// <summary>
        /// Creates a new inventory item for the specified equipment
        /// </summary>
        /// <param name="equipment">The equipment data to wrap</param>
        public InventoryItem(EquipmentData equipment)
        {
            Equipment = equipment;
            PosClaimInventory = new HashSet<(int, int)>();
        }

        /// <summary>
        /// Updates the grid positions this item occupies in the inventory
        /// </summary>
        /// <param name="posClaimInventory">The set of positions to claim</param>
        public void UpdatePosInInventory(HashSet<(int, int)> posClaimInventory)
        {
            PosClaimInventory.Clear();
            foreach (var pos in posClaimInventory)
            {
                PosClaimInventory.Add(pos);
            }
        }
        
        /// <summary>
        /// Gets the width of this item in grid cells
        /// </summary>
        public int Width => Equipment.Width;
        
        /// <summary>
        /// Gets the height of this item in grid cells
        /// </summary>
        public int Height => Equipment.Height;
    }
}