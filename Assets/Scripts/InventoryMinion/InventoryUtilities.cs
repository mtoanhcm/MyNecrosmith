using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Provides utility functions for inventory operations.
    /// Contains algorithms for finding space, optimizing layout, and other common tasks.
    /// </summary>
    public static class InventoryUtilities
    {
        /// <summary>
        /// Finds an empty space for an item in the grid
        /// </summary>
        /// <param name="item">The item to place</param>
        /// <param name="grid">The grid of cells</param>
        /// <param name="rows">Number of rows in the grid</param>
        /// <param name="columns">Number of columns in the grid</param>
        /// <returns>The position where the item can be placed, or null if no space is available</returns>
        public static Vector2Int? FindEmptySpaceForItem(InventoryItem item, GridCell[,] grid, int rows, int columns)
        {
            if (item == null || grid == null)
                return null;
                
            int width = item.Width;
            int height = item.Height;
            
            // Try each possible position
            for (int r = 0; r <= rows - height; r++)
            {
                for (int c = 0; c <= columns - width; c++)
                {
                    // Check if all required cells are available
                    bool canPlace = true;
                    
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (!grid[r + y, c + x].CanPlaceItem())
                            {
                                canPlace = false;
                                break;
                            }
                        }
                        
                        if (!canPlace)
                            break;
                    }
                    
                    if (canPlace)
                    {
                        return new Vector2Int(r, c);
                    }
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Optimizes the layout of items in the inventory
        /// </summary>
        /// <param name="items">The items to optimize</param>
        /// <param name="grid">The grid of cells</param>
        /// <param name="rows">Number of rows in the grid</param>
        /// <param name="columns">Number of columns in the grid</param>
        /// <returns>True if optimization was successful, false otherwise</returns>
        public static bool OptimizeInventoryLayout(List<InventoryItem> items, GridCell[,] grid, int rows, int columns)
        {
            if (items == null || grid == null)
                return false;
                
            // Sort items by size (largest first)
            List<InventoryItem> sortedItems = SortItemsBySize(items);
            
            // Reset the grid
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (grid[r, c].State != GridCell.CellState.Locked)
                    {
                        grid[r, c].SetEmpty();
                    }
                }
            }
            
            // Place each item
            foreach (var item in sortedItems)
            {
                Vector2Int? position = FindEmptySpaceForItem(item, grid, rows, columns);
                
                if (position == null)
                    return false; // Failed to place an item
                    
                // Update the grid
                UpdateGridWithItem(item, position.Value, grid);
            }
            
            return true;
        }
        
        /// <summary>
        /// Updates the grid with an item at the specified position
        /// </summary>
        /// <param name="item">The item to place</param>
        /// <param name="position">The position to place the item</param>
        /// <param name="grid">The grid of cells</param>
        private static void UpdateGridWithItem(InventoryItem item, Vector2Int position, GridCell[,] grid)
        {
            // Mark cells as occupied
            for (int y = 0; y < item.Height; y++)
            {
                for (int x = 0; x < item.Width; x++)
                {
                    grid[position.x + y, position.y + x].SetOccupied(item.GetHashCode().ToString());
                }
            }
            
            // Update the item's position
            HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();
            
            for (int y = 0; y < item.Height; y++)
            {
                for (int x = 0; x < item.Width; x++)
                {
                    occupiedPositions.Add((position.x + y, position.y + x));
                }
            }
            
            item.UpdatePosInInventory(occupiedPositions);
        }
        
        /// <summary>
        /// Sorts items by size (largest first)
        /// </summary>
        /// <param name="items">The items to sort</param>
        /// <returns>Sorted list of items</returns>
        public static List<InventoryItem> SortItemsBySize(List<InventoryItem> items)
        {
            List<InventoryItem> sortedItems = new List<InventoryItem>(items);
            
            // Sort by total area (width * height), then by width, then by height
            sortedItems.Sort((a, b) =>
            {
                int areaA = a.Width * a.Height;
                int areaB = b.Width * b.Height;
                
                if (areaA != areaB)
                    return areaB.CompareTo(areaA); // Descending order
                    
                if (a.Width != b.Width)
                    return b.Width.CompareTo(a.Width); // Descending order
                    
                return b.Height.CompareTo(a.Height); // Descending order
            });
            
            return sortedItems;
        }
        
        /// <summary>
        /// Gets a list of all cells in a rectangular area
        /// </summary>
        /// <param name="topLeft">Top-left position</param>
        /// <param name="width">Width in cells</param>
        /// <param name="height">Height in cells</param>
        /// <returns>List of positions</returns>
        public static List<Vector2Int> GetCellsInRect(Vector2Int topLeft, int width, int height)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells.Add(new Vector2Int(topLeft.x + y, topLeft.y + x));
                }
            }
            
            return cells;
        }
        
        /// <summary>
        /// Checks if all cells in a rectangular area are available
        /// </summary>
        /// <param name="topLeft">Top-left position</param>
        /// <param name="width">Width in cells</param>
        /// <param name="height">Height in cells</param>
        /// <param name="grid">The grid of cells</param>
        /// <param name="rows">Number of rows in the grid</param>
        /// <param name="columns">Number of columns in the grid</param>
        /// <returns>True if all cells are available, false otherwise</returns>
        public static bool AreCellsAvailable(Vector2Int topLeft, int width, int height, GridCell[,] grid, int rows, int columns)
        {
            // Check if the area is within bounds
            if (topLeft.x < 0 || topLeft.y < 0 || 
                topLeft.x + height > rows || topLeft.y + width > columns)
                return false;
                
            // Check if all cells are available
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!grid[topLeft.x + y, topLeft.y + x].CanPlaceItem())
                        return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Calculates the maximum carry capacity based on character stats
        /// </summary>
        /// <param name="strengthStat">Strength stat</param>
        /// <param name="enduranceStat">Endurance stat</param>
        /// <returns>Maximum carry capacity</returns>
        public static int CalculateMaxCarryCapacity(int strengthStat, int enduranceStat)
        {
            // This is a sample formula, adjust as needed for your game
            return 50 + (strengthStat * 2) + (enduranceStat * 1);
        }
        
        /// <summary>
        /// Calculates the total weight of all items in the inventory
        /// </summary>
        /// <param name="items">The items in the inventory</param>
        /// <returns>Total weight</returns>
        public static int CalculateTotalWeight(List<InventoryItem> items)
        {
            if (items == null)
                return 0;
                
            int totalWeight = 0;
            
            foreach (var item in items)
            {
                totalWeight += item.Equipment.LoadPoint;
            }
            
            return totalWeight;
        }
        
        /// <summary>
        /// Checks if a point is within a rectangular area
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="topLeft">Top-left position of the area</param>
        /// <param name="width">Width of the area</param>
        /// <param name="height">Height of the area</param>
        /// <returns>True if the point is within the area, false otherwise</returns>
        public static bool IsPointInRect(Vector2Int point, Vector2Int topLeft, int width, int height)
        {
            return point.x >= topLeft.x && point.x < topLeft.x + height &&
                   point.y >= topLeft.y && point.y < topLeft.y + width;
        }
        
        /// <summary>
        /// Gets the item at the specified position
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <param name="items">The items in the inventory</param>
        /// <returns>The item at the position, or null if none found</returns>
        public static InventoryItem GetItemAtPosition(Vector2Int position, List<InventoryItem> items)
        {
            if (items == null)
                return null;
                
            foreach (var item in items)
            {
                if (item.PosClaimInventory.Contains((position.x, position.y)))
                    return item;
            }
            
            return null;
        }
        
        /// <summary>
        /// Finds an optimal position for a new item
        /// </summary>
        /// <param name="item">The item to place</param>
        /// <param name="grid">The grid of cells</param>
        /// <param name="rows">Number of rows in the grid</param>
        /// <param name="columns">Number of columns in the grid</param>
        /// <param name="preferredPosition">Preferred position, if any</param>
        /// <returns>The optimal position, or null if no position is available</returns>
        public static Vector2Int? FindOptimalPosition(InventoryItem item, GridCell[,] grid, int rows, int columns, Vector2Int? preferredPosition = null)
        {
            // If a preferred position is specified and available, use it
            if (preferredPosition.HasValue && 
                AreCellsAvailable(preferredPosition.Value, item.Width, item.Height, grid, rows, columns))
            {
                return preferredPosition.Value;
            }
            
            // Try to find a position close to the center
            Vector2Int center = new Vector2Int(rows / 2, columns / 2);
            
            // Try positions in order of increasing distance from center
            for (int distance = 0; distance < Mathf.Max(rows, columns); distance++)
            {
                // Check positions at this distance from center
                for (int y = -distance; y <= distance; y++)
                {
                    for (int x = -distance; x <= distance; x++)
                    {
                        // Only check positions at exact distance (Manhattan distance)
                        if (Mathf.Abs(x) + Mathf.Abs(y) != distance)
                            continue;
                            
                        Vector2Int position = new Vector2Int(center.x + y, center.y + x);
                        
                        if (AreCellsAvailable(position, item.Width, item.Height, grid, rows, columns))
                            return position;
                    }
                }
            }
            
            // If no optimal position found, fall back to any available position
            return FindEmptySpaceForItem(item, grid, rows, columns);
        }
    }
}