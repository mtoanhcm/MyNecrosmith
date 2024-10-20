using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Character;
using UnityEngine;

namespace UI
{
    public class UIInventoryPanelCellHandle
    {
        private UIInventoryCell[,] cells;
        public HashSet<(int, int)> InventoryCellHash { get; private set; }
        
        public UIInventoryPanelCellHandle(int width, int height)
        {
            cells = new UIInventoryCell[width, height];
            InventoryCellHash = new HashSet<(int, int)>();
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    InventoryCellHash.Add((i, j));
                }
            }
        }

        public void SetUIInventoryCell(int x, int y, UIInventoryCell cell)
        {
            cells[x, y] = cell;
        }

        public void SetItemForCell(HashSet<(int,int)> claimPos, string itemClaimID)
        {
            foreach (var pos in claimPos)
            {
                cells[pos.Item1, pos.Item2].SetItemClaim(itemClaimID);   
            }
        }

        public void SetLockCell(int x, int y, bool isLocked)
        {
            cells[x, y].SetLockCell(isLocked);
        }

        public void LockAllCells(bool isLocked = true)
        {
            foreach (var pos in InventoryCellHash)
            {
                SetLockCell(pos.Item1, pos.Item2, isLocked);
            }
        }

        public void ResetAllCellHoverState()
        {
            foreach (var pos in InventoryCellHash)
            {
                cells[pos.Item1, pos.Item2].OnExitHoverOnCell();
            }
        }

        public Vector3 GetCenterPositionOfCellArea(int startRow, int startColumn, int areaWidth, int areaHeight)
        {
            var topLeftCell = cells[startRow, startColumn];
            var bottomRightCell = cells[startRow + areaHeight - 1, startColumn + areaWidth - 1];
            
            var topLeftPosition = topLeftCell.transform.position;
            var bottomRightPosition = bottomRightCell.transform.position;
            
            return (topLeftPosition + bottomRightPosition) / 2f;
        }
        
        public bool CanPlaceEquipmentOnCells(UIItemDragAndDrop dragItem, RectTransform inventoryRect, out HashSet<(int, int)> claimPos)
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

                    if (!TryGettingInventoryPosFromWordPos(dragItem.Cells[i, j].transform.position, inventoryRect,
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
        
        public void CheckHoverCell(UIItemDragAndDrop dragItem, RectTransform inventoryRect)
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

                    if (TryGettingInventoryPosFromWordPos(dragItem.Cells[i, j].transform.position, inventoryRect,
                            out var pos))
                    {
                        cells[pos.x, pos.y].OnHoverOnCell();
                    }
                }
            }
        }

        private bool TryGettingInventoryPosFromWordPos(Vector2 worldPos, RectTransform inventoryRect, out Vector2Int pos)
        {
            pos = Vector2Int.zero;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRect, worldPos, null, out var localPosition);
            if (!inventoryRect.rect.Contains(localPosition))
            {
                return false;
            }
    
            var adjustedX = localPosition.x + inventoryRect.rect.width * 0.5f;
            var adjustedY = inventoryRect.rect.height * 0.5f - localPosition.y;

            var column = Mathf.FloorToInt(adjustedX / (InventoryParam.CELL_SIZE + InventoryParam.CELL_SPACING));
            var row = Mathf.FloorToInt(adjustedY / (InventoryParam.CELL_SIZE + InventoryParam.CELL_SPACING));

            // check position in limit of inventory
            if (row < 0 || row >= InventoryParam.MAX_ROW || column < 0 || column >= InventoryParam.MAX_COLUMN)
            {
                return false;
            }
            
            pos = new Vector2Int(row, column);
            
            return true;
        }
    }   
}
