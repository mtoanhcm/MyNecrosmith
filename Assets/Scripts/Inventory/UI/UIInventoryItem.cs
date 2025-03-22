using System;
using Equipment;
using GameUtility.UI;
using Minion.Inventory;
using Observer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : UIDragHandle<UIInventoryItem>
    {
        public UIInventoryItemCell[,] Cells => cells;
        public RectTransform MyRect => myRectTransform;
        public InventoryItem InventoryItem => inventoryItem;
        
        public Action<UIInventoryItem> OnCheckItemHover { get; set; }
        public Action<EquipmentData> OnPlaceInMinionInventorySuccess {get; set;}
        
        [SerializeField] private Image iconImg;
        [SerializeField] private GridLayoutGroup cellParent;
        [SerializeField] private UIInventoryItemCell cellPrefab;
        
        private RectTransform myRectTransform;
        private InventoryItem inventoryItem;
        private UIInventoryItemCell[,] cells;
        private bool isHoldingItem;
        private int delayFrameToUpdateHoverEvent;

        private void Awake()
        {
            myRectTransform = GetComponent<RectTransform>();

            OnPickItemAction += uiItem =>
            {
                isHoldingItem = true;
                delayFrameToUpdateHoverEvent = 0;
            };

            // OnReleaseItemAction = uiItem =>
            // {
            //     isHoldingItem = false;
            //
            //     Debug.Log("Send place item event");
            //     EventManager.Instance.TriggerEvent(new EventData.OnPlaceInventoryItemUI()
            //     {
            //         UIItem = uiItem,
            //         OnPlaceItemInMinionInventorySuccess = OnPlaceInMinionInventorySuccess
            //     });
            // };
        }

        private void LateUpdate()
        {
            if (!isHoldingItem)
            {
                return;
            }
            
            transform.position = Mouse.current.position.ReadValue();
            
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 5 == 0)
            {
                //EventManager.Instance.TriggerEvent(new EventData.OnDraggingInventoryItemUI(){ UIItem = this});
                OnCheckItemHover?.Invoke(this);
            }
        }

        public void Init(EquipmentData data)
        {
            SetEmptyCells();
            SetItemData(data);
        }

        public void SetHoldingItem(bool isHolding)
        {
            isHoldingItem = isHolding;
            if (isHoldingItem)
            {
                delayFrameToUpdateHoverEvent = 0;
            }
        }

        private void SetItemData(EquipmentData equipmentData)
        {
            inventoryItem = new InventoryItem(equipmentData);
            iconImg.sprite = equipmentData.IconSpr;
            
            var scaleSize = new Vector2(
                MinionInventoryParam.CELL_SIZE * equipmentData.Width + (MinionInventoryParam.CELL_SPACING * (equipmentData.Width - 1)), 
                MinionInventoryParam.CELL_SIZE * equipmentData.Height + (MinionInventoryParam.CELL_SPACING * (equipmentData.Height - 1)));

            myRectTransform.sizeDelta = scaleSize;

            cellParent.cellSize = new Vector2(MinionInventoryParam.CELL_SIZE, MinionInventoryParam.CELL_SIZE);

            ToggleCells(equipmentData);
        }
        
        private void ToggleCells(EquipmentData equipment)
        {
            for (var i = 0; i < MinionInventoryParam.MAX_EQUIPMENT_WIDTH; i++)
            {
                for (var j = 0; j < MinionInventoryParam.MAX_EQUIPMENT_HEIGHT; j++)
                {
                    cells[i, j].SetVisible(i < equipment.Width && j < equipment.Height);
                }
            }
        }
        
        private void SetEmptyCells()
        {
            if (cells != null && cells.Length == 0)
            {
                ResetCellsToEmpty();
                return;
            }

            CreateEmptyCells();

            return;

            void CreateEmptyCells()
            {
                cellParent.spacing = new Vector2(MinionInventoryParam.CELL_SPACING, MinionInventoryParam.CELL_SPACING);
                cells = new UIInventoryItemCell[MinionInventoryParam.MAX_EQUIPMENT_WIDTH, MinionInventoryParam.MAX_EQUIPMENT_WIDTH];
                for (var i = 0; i < MinionInventoryParam.MAX_EQUIPMENT_WIDTH; i++)
                {
                    for (var j = 0; j < MinionInventoryParam.MAX_EQUIPMENT_HEIGHT; j++)
                    {
                        var cell = Instantiate(cellPrefab, cellParent.transform);
                        cell.SetVisible(false);

                        cells[i, j] = cell;
                    }
                }
            }

            void ResetCellsToEmpty()
            {
                for (var i = 0; i < cells.GetLength(0); i++)
                {
                    for (var j = 0; j < cells.GetLength(1); j++)
                    {
                        cells[i, j].SetVisible(false);
                    }
                }
            }
        }
    }   
}
