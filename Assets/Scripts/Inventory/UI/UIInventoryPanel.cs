using System;
using Observer;
using UnityEngine;
using Character;
using Ultility;
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
            EventManager.Instance.StartListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
        }

        private void OnDisable()
        {
            LockAllCells();
            EventManager.Instance.StopListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
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
            if (!data.ItemDragRec.IsWorldOverlap(inventoryRect))
            {
                return;
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
    }   
}
