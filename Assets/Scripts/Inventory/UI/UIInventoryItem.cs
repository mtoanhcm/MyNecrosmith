using System;
using Equipment;
using Minion.Inventory;
using Observer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UIInventoryItemCell[,] Cells => cells;
        
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
                //EventManager.Instance.TriggerEvent(new EventData.OnDraggingInventoryItemUI(){ Item = this});
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
                //EventManager.Instance.TriggerEvent(new EventData.OnDraggingInventoryItemUI(){ Item = this});
                delayFrameToUpdateHoverEvent = 0;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            //transform.position = Mouse.current.position.ReadValue();
    
            //EventManager.Instance.TriggerEvent(new EventData.OnDraggingInventoryItemUI(){ Item = this});
    
            isHoldingItem = true;
            delayFrameToUpdateHoverEvent = 0;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHoldingItem = false;

            EventManager.Instance.TriggerEvent(new EventData.OnPlaceInventoryItemUI()
            {
                Item = this,
                OnPlaceItemSuccess = OnPlaceEquipmentSuccessInInventory
            });

            void OnPlaceEquipmentSuccessInInventory(EquipmentData equipmentData)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnRemoveEquipmentFromPlayerStorage()
                {
                    EquipmentData = equipmentData
                });
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            delayFrameToUpdateHoverEvent = 0;
            transform.position = Mouse.current.position.ReadValue();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // transform.position = Mouse.current.position.ReadValue();
            //
            // delayFrameToUpdateHoverEvent++;
            // if (delayFrameToUpdateHoverEvent % 5 == 0)
            // {
            //     EventManager.Instance.TriggerEvent(new EventData.OnDraggingInventoryItemUI(){ Item = this});
            // }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
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
