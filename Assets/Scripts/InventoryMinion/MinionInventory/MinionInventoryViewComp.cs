using System.Collections.Generic;
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
using UnityEngine;

namespace Inventory.UI
{
    /// <summary>
    /// Handles operations on inventory panel cells.
    /// Manages cell states, highlighting, and item placement validation.
    /// </summary>
    public class UIInventoryPanelCellHandle
    {
        /// <summary>
        /// 2D array of inventory cells
        /// </summary>
        private UIInventoryCell[,] cells;
        
        /// <summary>
        /// Set of all positions in the inventory grid
        /// </summary>
        public HashSet<(int, int)> InventoryCellHash { get; private set; }
        
        /// <summary>
        /// Creates a new cell handle with the specified dimensions
        /// </summary>
        /// <param name="width">Number of rows</param>
        /// <param name="height">Number of columns</param>
        public UIInventoryPanelCellHandle(int width, int height)
========
using Inventory.UI;
using Minion.Inventory.UI;
using UnityEngine;

namespace Minion.Inventory
{
    public class MinionInventoryViewComp
    {
        private UIMinionInventoryCell[,] cells;
        public HashSet<(int, int)> InventoryCellHash { get; private set; }

        public MinionInventoryViewComp(int width, int height)
>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
        {
            cells = new UIMinionInventoryCell[width, height];
            InventoryCellHash = new HashSet<(int, int)>();
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
            
            // Initialize the hash of all grid positions
========

>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    InventoryCellHash.Add((i, j));
                }
            }
        }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
        
        /// <summary>
        /// Sets a UI cell at the specified position
        /// </summary>
        /// <param name="x">Row index</param>
        /// <param name="y">Column index</param>
        /// <param name="cell">The cell to set</param>
        public void SetUIInventoryCell(int x, int y, UIInventoryCell cell)
        {
            cells[x, y] = cell;
        }
        
        /// <summary>
        /// Sets cells as claimed by an item
        /// </summary>
        /// <param name="claimPos">Positions claimed by the item</param>
        /// <param name="itemClaimID">ID of the claiming item</param>
        public void SetItemForCell(HashSet<(int,int)> claimPos, string itemClaimID)
========

        public void SetUIInventoryCell(int x, int y, UIMinionInventoryCell cell)
        {
            cells[x, y] = cell;
        }

        public void SetItemForCell(HashSet<(int, int)> claimPos, string itemClaimID)
>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
        {
            foreach (var pos in claimPos)
            {
                cells[pos.Item1, pos.Item2].SetItemClaim(itemClaimID);
            }
        }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
        
        /// <summary>
        /// Removes an item's claim on cells
        /// </summary>
        /// <param name="itemClaimID">ID of the item</param>
        public void RemoveItemForcell(string itemClaimID)
========

        public void RemoveItemForCell(string itemClaimID)
>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
        {
            foreach (var pos in InventoryCellHash)
            {
                var cell = cells[pos.Item1, pos.Item2];
                if (cell.IsClaimed && itemClaimID == cell.ItemClaimID)
                {
                    cell.SetItemClaim(string.Empty);
                }
            }
        }
        
        /// <summary>
        /// Sets whether a cell is locked
        /// </summary>
        /// <param name="x">Row index</param>
        /// <param name="y">Column index</param>
        /// <param name="isLocked">Whether to lock the cell</param>
        public void SetLockCell(int x, int y, bool isLocked)
        {
            cells[x, y].SetLockCell(isLocked);
        }
        
        /// <summary>
        /// Locks or unlocks all cells
        /// </summary>
        /// <param name="isLocked">Whether to lock all cells</param>
        public void LockAllCells(bool isLocked = true)
        {
            foreach (var pos in InventoryCellHash)
            {
                SetLockCell(pos.Item1, pos.Item2, isLocked);
            }
        }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
        
        /// <summary>
        /// Resets the hover state of all cells
        /// </summary>
========

>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
        public void ResetAllCellHoverState()
        {
            foreach (var pos in InventoryCellHash)
            {
                cells[pos.Item1, pos.Item2].OnExitHoverOnCell();
            }
        }
        
        /// <summary>
        /// Gets the center position of a rectangular area of cells
        /// </summary>
        /// <param name="startRow">Starting row</param>
        /// <param name="startColumn">Starting column</param>
        /// <param name="areaWidth">Width in cells</param>
        /// <param name="areaHeight">Height in cells</param>
        /// <returns>Center position of the area</returns>
        public Vector3 GetCenterPositionOfCellArea(int startRow, int startColumn, int areaWidth, int areaHeight)
        {
            var topLeftCell = cells[startRow, startColumn];
            var bottomRightCell = cells[startRow + areaHeight - 1, startColumn + areaWidth - 1];

            var topLeftPosition = topLeftCell.transform.position;
            var bottomRightPosition = bottomRightCell.transform.position;

            return (topLeftPosition + bottomRightPosition) / 2f;
        }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
        
        /// <summary>
        /// Checks if an equipment item can be placed on the inventory cells
        /// </summary>
        /// <param name="dragItem">The item being dragged</param>
        /// <param name="inventoryRect">The inventory panel rectangle</param>
        /// <param name="claimPos">Output parameter for claimed positions</param>
        /// <returns>True if placement is valid, false otherwise</returns>
        public bool CanPlaceEquipmentOnCells(UIInventoryItem dragItem, RectTransform inventoryRect, out HashSet<(int, int)> claimPos)
========

        public bool CanPlaceEquipmentOnCells(UIInventoryItem dragItem, RectTransform inventoryRect,
            out HashSet<(int, int)> claimPos)
>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
        {
            claimPos = new HashSet<(int, int)>();

            //Check each cell of drag item, if it is not on valid cell, return false
            for (var i = 0; i < dragItem.Cells.GetLength(0); i++)
            {
                for (var j = 0; j < dragItem.Cells.GetLength(1); j++)
                {
                    var cellDrag = dragItem.Cells[i, j];
                    if (!cellDrag.gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    if (!TryGettingInventoryPosFromWordPos(cellDrag.transform.position, inventoryRect,
                            out var pos))
                    {
                        return false;
                    }

                    if (cells[pos.x, pos.y].IsLocked || cells[pos.x, pos.y].IsClaimed)
                    {
                        return false;
                    }

                    claimPos.Add((pos.x, pos.y));
                }
            }

            return true;
        }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
        
        /// <summary>
        /// Highlights cells when a drag item is hovering over them
        /// </summary>
        /// <param name="dragItem">The item being dragged</param>
        /// <param name="inventoryRect">The inventory panel rectangle</param>
========

>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
        public void CheckHoverCell(UIInventoryItem dragItem, RectTransform inventoryRect)
        {
            for (var i = 0; i < dragItem.Cells.GetLength(0); i++)
            {
                for (var j = 0; j < dragItem.Cells.GetLength(1); j++)
                {
                    var cellDrag = dragItem.Cells[i, j];
                    if (!cellDrag.gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    if (TryGettingInventoryPosFromWordPos(cellDrag.transform.position, inventoryRect,
                            out var pos))
                    {
                        cells[pos.x, pos.y].OnHoverOnCell();
                    }
                }
            }
        }

<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
        /// <summary>
        /// Attempts to convert a world position to an inventory grid position
        /// </summary>
        /// <param name="worldPos">The world position</param>
        /// <param name="inventoryRect">The inventory panel rectangle</param>
        /// <param name="pos">Output parameter for the grid position</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        private bool TryGettingInventoryPosFromWordPos(Vector2 worldPos, RectTransform inventoryRect, out Vector2Int pos)
        {
            pos = Vector2Int.zero;
            
            // Convert screen position to local position within inventory rect
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRect, worldPos, null, out var localPosition);
========
        public void ClearInventoryCells()
        {
            ResetAllCellHoverState();

            foreach (var pos in InventoryCellHash)
            {
                cells[pos.Item1, pos.Item2].SetItemClaim(string.Empty);
            }
        }
        
        private bool TryGettingInventoryPosFromWordPos(Vector2 worldPos, RectTransform inventoryRect,
            out Vector2Int pos)
        {
            pos = Vector2Int.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRect, worldPos, null,
                out var localPosition);
>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
            if (!inventoryRect.rect.Contains(localPosition))
            {
                return false;
            }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
    
            // Adjust for pivot and calculate grid position
            var adjustedX = localPosition.x + inventoryRect.rect.width * inventoryRect.pivot.x;
            var adjustedY = inventoryRect.rect.height * (1 - inventoryRect.pivot.y) - localPosition.y;
            
            var column = Mathf.FloorToInt(adjustedX / (InventoryConstants.CELL_SIZE + InventoryConstants.CELL_SPACING));
            var row = Mathf.FloorToInt(adjustedY / (InventoryConstants.CELL_SIZE + InventoryConstants.CELL_SPACING));
            
            // Check position is within inventory bounds
            if (row < 0 || row >= InventoryConstants.MAX_ROW || column < 0 || column >= InventoryConstants.MAX_COLUMN)
            {
                return false;
            }
            
========

            var adjustedX = localPosition.x + inventoryRect.rect.width * inventoryRect.pivot.x;
            var adjustedY = inventoryRect.rect.height * (1 - inventoryRect.pivot.y) - localPosition.y;

            var column = Mathf.FloorToInt(adjustedX / (MinionInventoryParam.CELL_SIZE + MinionInventoryParam.CELL_SPACING));
            var row = Mathf.FloorToInt(adjustedY / (MinionInventoryParam.CELL_SIZE + MinionInventoryParam.CELL_SPACING));

            // check position in limit of inventory
            if (row < 0 || row >= MinionInventoryParam.MAX_ROW || column < 0 || column >= MinionInventoryParam.MAX_COLUMN)
            {
                return false;
            }

            //Debug.Log($"{row} --- {column}");

>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
            pos = new Vector2Int(row, column);

            return true;
        }
<<<<<<<< HEAD:Assets/Scripts/InventoryMinion/UI/UIInventoryPanelCellHandle.cs
    }   
========
    }

>>>>>>>> release/Prototype:Assets/Scripts/InventoryMinion/MinionInventory/MinionInventoryViewComp.cs
}