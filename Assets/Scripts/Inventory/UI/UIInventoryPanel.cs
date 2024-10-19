using System;
using Observer;
using UnityEngine;
using Character;
using Ultility;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace UI
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup cellGridContainer;
        [SerializeField] private UIInventoryCell cellPrefab;
        
        private UIInventoryCell[,] cells;
        private RectTransform inventoryRect;

        private void Awake()
        {
            InitInventorySize();
            InitInventoryEmptyCell();

            return;

            void InitInventorySize()
            {
                inventoryRect = cellGridContainer.GetComponent<RectTransform>();
                inventoryRect.sizeDelta = new Vector2(
                    InventoryParam.CELL_SIZE * InventoryParam.MAX_ROW +
                    (cellGridContainer.spacing.x * (InventoryParam.MAX_ROW - 1)),
                    InventoryParam.CELL_SIZE * InventoryParam.MAX_COLUMN +
                    (cellGridContainer.spacing.x * (InventoryParam.MAX_COLUMN - 1))
                );
            
                cellGridContainer.spacing = new Vector2(InventoryParam.CELL_SPACING, InventoryParam.CELL_SPACING);
                cellGridContainer.cellSize = new Vector2(InventoryParam.CELL_SIZE, InventoryParam.CELL_SIZE);
            }

            void InitInventoryEmptyCell()
            {
                cells = new UIInventoryCell[InventoryParam.MAX_ROW, InventoryParam.MAX_COLUMN];
                for (var i = 0; i < InventoryParam.MAX_ROW; i++)
                {
                    for (var j = 0; j < InventoryParam.MAX_COLUMN; j++)
                    {
                        var cell = Instantiate(cellPrefab, cellGridContainer.transform).GetComponent<UIInventoryCell>();
                        cell.Init(i,j);
                    
                        cells[i,j] = cell;
                    }
                }
            }
        }

        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
        }

        private void OnDisable()
        {
            LockAllCells();
            EventManager.Instance?.StopListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
        }

        public void OpenInventory(Inventory characterInventory)
        {
            LockAllCells();
            
            for (var i = 0; i < characterInventory.Row; i++)
            {
                for (var j = 0; j < characterInventory.Column; j++)
                {
                    var posX = InventoryParam.MAX_ROW / 2 +  (i - characterInventory.Row / 2);
                    var posY = InventoryParam.MAX_COLUMN / 2 + (j - characterInventory.Column / 2);
                    
                    cells[posX, posY].SetLockCell(false);
                }
            }
        }

        private void OnCheckDraggingEquipmentHoverInventory(EventData.DraggingEquipment data)
        {
            if (!data.DragItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return;
            }

            for (var i = 0; i < data.DragItem.Cells.GetLength(0); i++)
            {
                for (var j = 0; j < data.DragItem.Cells.GetLength(1); j++)
                {
                    var cellDrag = data.DragItem.Cells[i, j];
                    if (!cellDrag.gameObject.activeSelf)
                    {
                        continue;
                    }
                    DetectHoverOnCell(data.DragItem.Cells[i, j].transform.position);
                }
            }
        }

        private void LockAllCells()
        {
            for (var i = 0; i < InventoryParam.MAX_ROW; i++)
            {
                for (var j = 0; j < InventoryParam.MAX_COLUMN; j++)
                {
                    cells[i,j].SetLockCell(true);
                }
            }
        }

        private void DetectHoverOnCell(Vector2 worldPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRect, worldPos, null, out var localPosition);
            if (!inventoryRect.rect.Contains(localPosition))
            {
                return;
            }
    
            // Chuyển đổi hệ tọa độ từ local sang tọa độ của RectTransform
            var adjustedX = localPosition.x + inventoryRect.rect.width * 0.5f;
            var adjustedY = inventoryRect.rect.height * 0.5f - localPosition.y;

            var column = Mathf.FloorToInt(adjustedX / (InventoryParam.CELL_SIZE + InventoryParam.CELL_SPACING));
            var row = Mathf.FloorToInt(adjustedY / (InventoryParam.CELL_SIZE + InventoryParam.CELL_SPACING));

            // Kiểm tra giá trị row và column có nằm trong giới hạn của mảng cells hay không
            if (row < 0 || row >= InventoryParam.MAX_ROW || column < 0 || column >= InventoryParam.MAX_COLUMN)
            {
                return;
            }

            // Nếu cell không bị khóa, thực hiện hành động khác
            Debug.Log($"Hovered on cell at row: {row}, column: {column}");
        }
    }   
}
