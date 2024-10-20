using System.Collections.Generic;
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

        public void SetLockCell(int x, int y, bool isLocked)
        {
            cells[x, y].SetLockCell(isLocked);
        }

        public void LockAllCells(bool isLocked = true)
        {
            foreach (var pos in InventoryCellHash)
            {
                cells[pos.Item1, pos.Item2].SetLockCell(isLocked);
            }
        }

        public void ResetAllCellHoverState()
        {
            foreach (var pos in InventoryCellHash)
            {
                cells[pos.Item1, pos.Item2].OnExitHoverOnCell();
            }
        }

        private void HoverOnCell(int x, int y)
        {
            cells[x, y].OnHoverOnCell();
        }
        
        public void CheckHoverCell(UIItemDragAndDrop dragItem, RectTransform inventoryRect)
        {
            for (var i = 0; i < dragItem.Cells.GetLength(0); i++)
            {
                for (var j = 0; j < dragItem.Cells.GetLength(1); j++)
                {
                    var cellDrag = dragItem.Cells[i, j];
                    if (!cellDrag.gameObject.activeSelf)
                    {
                        continue;
                    }
                    
                    DetectHoverOnCell(dragItem.Cells[i, j].transform.position);
                }
            }

            return;
            
            void DetectHoverOnCell(Vector2 worldPos)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRect, worldPos, null, out var localPosition);
                if (!inventoryRect.rect.Contains(localPosition))
                {
                    return;
                }
    
                var adjustedX = localPosition.x + inventoryRect.rect.width * 0.5f;
                var adjustedY = inventoryRect.rect.height * 0.5f - localPosition.y;

                var column = Mathf.FloorToInt(adjustedX / (InventoryParam.CELL_SIZE + InventoryParam.CELL_SPACING));
                var row = Mathf.FloorToInt(adjustedY / (InventoryParam.CELL_SIZE + InventoryParam.CELL_SPACING));

                // check position in limit of inventory
                if (row < 0 || row >= InventoryParam.MAX_ROW || column < 0 || column >= InventoryParam.MAX_COLUMN)
                {
                    return;
                }

                HoverOnCell(row, column);
            }
        }
    }   
}
