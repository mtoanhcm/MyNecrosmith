using System.Linq;
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
        [SerializeField] private Transform itemContainer;
        [SerializeField] private UIInventoryCell cellPrefab;
        [SerializeField] private UIInventoryItem itemPrefab;
        
        private RectTransform inventoryRect;
        
        private UIInventoryPanelCellHandle cellHandle;
        private UIInventoryPanelEquipmentHandle equipmentHandle;

        private void Awake()
        {
            InitInventorySize();
            InitInventoryEmptyCell();

            equipmentHandle = new UIInventoryPanelEquipmentHandle();   
            
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
                cellHandle = new UIInventoryPanelCellHandle(InventoryParam.MAX_ROW, InventoryParam.MAX_COLUMN);
                
                foreach (var pos in cellHandle.InventoryCellHash)
                {
                    var cell = Instantiate(cellPrefab, cellGridContainer.transform).GetComponent<UIInventoryCell>();
                    cell.Init(pos.Item1, pos.Item2);
                    
                    cellHandle.SetUIInventoryCell(pos.Item1, pos.Item2, cell);
                }
            }
        }

        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
            EventManager.Instance?.StartListening<EventData.OnPlacingEquipment>(OnPlaceEquipmentToInventory);
        }

        private void OnDisable()
        {
            cellHandle.LockAllCells();
            EventManager.Instance?.StopListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
            EventManager.Instance?.StopListening<EventData.OnPlacingEquipment>(OnPlaceEquipmentToInventory);
        }

        public void OpenInventory(Inventory characterInventory)
        {
            cellHandle.LockAllCells();
            cellHandle.ResetAllCellHoverState();

            SetVisibleInventoryCell();
            
            equipmentHandle.SetInventoryItems(null);
            
            return;
            
            void SetVisibleInventoryCell()
            {
                for (var i = 0; i < characterInventory.Row; i++)
                {
                    for (var j = 0; j < characterInventory.Column; j++)
                    {
                        var posX = InventoryParam.MAX_ROW / 2 +  (i - characterInventory.Row / 2);
                        var posY = InventoryParam.MAX_COLUMN / 2 + (j - characterInventory.Column / 2);
                        cellHandle.SetLockCell(posX, posY, false);
                    }
                }
            }
        }

        private void OnCheckDraggingEquipmentHoverInventory(EventData.DraggingEquipment data)
        {
            if (!data.DragItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return;
            }

            cellHandle.ResetAllCellHoverState();
            cellHandle.CheckHoverCell(data.DragItem, inventoryRect);
        }
        
        private void OnPlaceEquipmentToInventory(EventData.OnPlacingEquipment data)
        {
            if (!cellHandle.CanPlaceEquipmentOnCells(data.ItemDrag, inventoryRect, out var claimPos))
            {
                return;
            }
            
            var uiItem = Instantiate(itemPrefab, itemContainer);
            uiItem.Init(data.Item.Equipment);
            uiItem.Item.UpdatePosInInventory(claimPos);

            var uiItemClaimPos = uiItem.Item.PosClaimInventory.First();
            uiItem.transform.position = cellHandle.GetCenterPositionOfCellArea(uiItemClaimPos.Item1,
                uiItemClaimPos.Item2, uiItem.Item.Equipment.Width, uiItem.Item.Equipment.Height);
            
            cellHandle.SetItemForCell(claimPos, uiItem.GetInstanceID().ToString());
            equipmentHandle.AddItemToInventory(uiItem);
        }
    }   
}
