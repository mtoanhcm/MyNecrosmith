using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Represents a single cell in the inventory grid.
    /// Tracks position, state, and the item (if any) occupying it.
    /// </summary>
    public class GridCell
    {
        /// <summary>
        /// Position of this cell in the grid (row, column)
        /// </summary>
        public Vector2Int Position { get; private set; }
        
        /// <summary>
        /// Current state of this cell
        /// </summary>
        public CellState State { get; private set; }
        
        /// <summary>
        /// ID of the item occupying this cell (if any)
        /// </summary>
        public string ItemClaimID { get; private set; }
        
        /// <summary>
        /// Enumeration of possible cell states
        /// </summary>
        public enum CellState 
        { 
            /// <summary>Cell is available for item placement</summary>
            Empty, 
            
            /// <summary>Cell is occupied by an item</summary>
            Occupied, 
            
            /// <summary>Cell is locked and cannot be used</summary>
            Locked 
        }
        
        /// <summary>
        /// Creates a new grid cell at the specified position
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        public GridCell(int row, int column)
        {
            Position = new Vector2Int(row, column);
            State = CellState.Empty;
            ItemClaimID = string.Empty;
        }
        
        /// <summary>
        /// Locks this cell, preventing item placement
        /// </summary>
        public void Lock()
        {
            State = CellState.Locked;
            ItemClaimID = string.Empty;
        }
        
        /// <summary>
        /// Unlocks this cell, allowing item placement
        /// </summary>
        public void Unlock()
        {
            State = CellState.Empty;
            ItemClaimID = string.Empty;
        }
        
        /// <summary>
        /// Sets this cell as occupied by the specified item
        /// </summary>
        /// <param name="itemId">ID of the occupying item</param>
        public void SetOccupied(string itemId)
        {
            State = CellState.Occupied;
            ItemClaimID = itemId;
        }
        
        /// <summary>
        /// Clears this cell, setting it to empty
        /// </summary>
        public void SetEmpty()
        {
            State = CellState.Empty;
            ItemClaimID = string.Empty;
        }
        
        /// <summary>
        /// Checks if an item can be placed in this cell
        /// </summary>
        /// <returns>True if the cell is empty and unlocked, false otherwise</returns>
        public bool CanPlaceItem()
        {
            return State == CellState.Empty;
        }
        
        /// <summary>
        /// Checks if this cell is claimed by a specific item
        /// </summary>
        /// <param name="itemId">ID of the item to check</param>
        /// <returns>True if the cell is occupied by the specified item</returns>
        public bool IsClaimedBy(string itemId)
        {
            return State == CellState.Occupied && ItemClaimID == itemId;
        }
    }
}