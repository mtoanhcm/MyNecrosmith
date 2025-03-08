using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Manages the grid logic for an inventory system.
    /// Handles the state of cells, checking for collisions, and finding valid item placements.
    /// </summary>
    public class InventoryGridManager
    {
        /// <summary>
        /// 2D array of grid cells representing the inventory grid
        /// </summary>
        private GridCell[,] grid;
        
        /// <summary>
        /// Number of rows in the grid
        /// </summary>
        private int rows;
        
        /// <summary>
        /// Number of columns in the grid
        /// </summary>
        private int columns;
        
        /// <summary>
        /// Creates a new grid manager with the specified dimensions
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        public InventoryGridManager(int rows, int columns)
        {
            this.rows = Mathf.Clamp(rows, InventoryConstants.MIN_ROW, InventoryConstants.MAX_ROW);
            this.columns = Mathf.Clamp(columns, InventoryConstants.MIN_COLUMN, InventoryConstants.MAX_COLUMN);
            
            InitializeGrid();
        }
        
        /// <summary>
        /// Initializes the grid with empty cells
        /// </summary>
        private void InitializeGrid()
        {
            grid = new GridCell[rows, columns];
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    grid[r, c] = new GridCell(r, c);
                }
            }
        }
        
        /// <summary>
        /// Checks if an item can be placed at the specified position
        /// </summary>
        /// <param name="item">The item to place</param>
        /// <param name="position">The top-left position to place the item</param>
        /// <returns>True if the item can be placed, false otherwise</returns>
        public bool CanPlaceItem(InventoryItem item, Vector2Int position)
        {
            if (item == null)
                return false;
                
            // Check if the item fits within the grid bounds
            if (!IsWithinBounds(position, item.Width, item.Height))
                return false;
                
            // Check if all required cells are available
            for (int r = 0; r < item.Height; r++)
            {
                for (int c = 0; c < item.Width; c++)
                {
                    int gridRow = position.x + r;
                    int gridCol = position.y + c;
                    
                    if (!grid[gridRow, gridCol].CanPlaceItem())
                        return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Checks if a position and dimensions are within the grid bounds
        /// </summary>
        /// <param name="position">Top-left position</param>
        /// <param name="width">Width in cells</param>
        /// <param name="height">Height in cells</param>
        /// <returns>True if within bounds, false otherwise</returns>
        public bool IsWithinBounds(Vector2Int position, int width, int height)
        {
            return position.x >= 0 && position.y >= 0 && 
                   position.x + height <= rows && position.y + width <= columns;
        }
        
        /// <summary>
        /// Finds an empty space for the specified item
        /// </summary>
        /// <param name="item">The item to place</param>
        /// <returns>Top-left position where the item can be placed, or null if no space is available</returns>
        public Vector2Int? FindEmptySpaceFor(InventoryItem item)
        {
            if (item == null)
                return null;
                
            // Try each possible position in the grid
            for (int r = 0; r < rows - item.Height + 1; r++)
            {
                for (int c = 0; c < columns - item.Width + 1; c++)
                {
                    Vector2Int position = new Vector2Int(r, c);
                    if (CanPlaceItem(item, position))
                        return position;
                }
            }
            
            return null; // No space found
        }
        
        /// <summary>
        /// Gets a list of all occupied positions in the grid
        /// </summary>
        /// <returns>List of occupied positions</returns>
        public List<Vector2Int> GetOccupiedPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (grid[r, c].State == GridCell.CellState.Occupied)
                        positions.Add(new Vector2Int(r, c));
                }
            }
            
            return positions;
        }
        
        /// <summary>
        /// Gets a list of item IDs that would overlap with the specified item at the given position
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="position">The position to check</param>
        /// <returns>List of overlapping item IDs</returns>
        public List<string> GetOverlappingItems(InventoryItem item, Vector2Int position)
        {
            if (item == null)
                return new List<string>();
                
            HashSet<string> itemIds = new HashSet<string>();
            
            // Check each cell the item would occupy
            for (int r = 0; r < item.Height; r++)
            {
                for (int c = 0; c < item.Width; c++)
                {
                    int gridRow = position.x + r;
                    int gridCol = position.y + c;
                    
                    // Skip if outside grid bounds
                    if (gridRow < 0 || gridRow >= rows || gridCol < 0 || gridCol >= columns)
                        continue;
                        
                    // If cell is occupied by another item, add its ID
                    if (grid[gridRow, gridCol].State == GridCell.CellState.Occupied && 
                        !string.IsNullOrEmpty(grid[gridRow, gridCol].ItemClaimID))
                    {
                        itemIds.Add(grid[gridRow, gridCol].ItemClaimID);
                    }
                }
            }
            
            return itemIds.ToList();
        }
        
        /// <summary>
        /// Updates the grid state based on the current items
        /// </summary>
        /// <param name="items">The collection of items in the inventory</param>
        public void UpdateGridState(List<InventoryItem> items)
        {
            // Reset all cells to empty
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (grid[r, c].State != GridCell.CellState.Locked)
                        grid[r, c].SetEmpty();
                }
            }
            
            // Mark cells as occupied by items
            if (items != null)
            {
                foreach (var item in items)
                {
                    foreach (var pos in item.PosClaimInventory)
                    {
                        int r = pos.Item1;
                        int c = pos.Item2;
                        
                        // Skip invalid positions
                        if (r < 0 || r >= rows || c < 0 || c >= columns)
                            continue;
                            
                        grid[r, c].SetOccupied(item.GetHashCode().ToString());
                    }
                }
            }
        }
        
        /// <summary>
        /// Locks a cell at the specified position
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <param name="isLocked">Whether to lock or unlock the cell</param>
        public void SetCellLock(int row, int column, bool isLocked)
        {
            if (row >= 0 && row < rows && column >= 0 && column < columns)
            {
                if (isLocked)
                    grid[row, column].Lock();
                else
                    grid[row, column].Unlock();
            }
        }
        
        /// <summary>
        /// Gets the cell at the specified position
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <returns>The grid cell at the specified position, or null if out of bounds</returns>
        public GridCell GetCell(int row, int column)
        {
            if (row >= 0 && row < rows && column >= 0 && column < columns)
                return grid[row, column];
            return null;
        }
        
        /// <summary>
        /// Gets the number of rows in the grid
        /// </summary>
        public int Rows => rows;
        
        /// <summary>
        /// Gets the number of columns in the grid
        /// </summary>
        public int Columns => columns;
    }
}