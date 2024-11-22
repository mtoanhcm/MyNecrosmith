using System;
using Character;
using Equipment;
using Observer;
using GameUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform MyRect => myRect;
        public InventoryItem Item { get; private set; }
        public UIItemDragCell[,] Cells => cells;

        [SerializeField] private Image equipmentIconImg;
        [SerializeField] private bool isInInventory;
        [SerializeField] private GridLayoutGroup layoutGroup;
        [SerializeField] private UIItemDragCell dragCellPrefab;

        private RectTransform myRect;
        private int delayFrameToUpdateHoverEvent;
        private UIItemDragCell[,] cells;
        private bool isInitCell;
        private bool isHoldingItem;
        
        private void Awake()
        {
            myRect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!isHoldingItem)
            {
                return;
            }
            
            transform.position = Mouse.current.position.ReadValue();
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 5 == 0)
            {
                EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ UIItem = this});
            }
        }
        
        public void Init(EquipmentData equipment)
        {
            Item = new InventoryItem(equipment);
            equipmentIconImg.sprite = equipment.IconSpr;
                
            CheckInitEmptyCell();
            SetItemDragData(equipment);
        }

        private void CheckInitEmptyCell()
        {
            if (isInitCell)
            {
                return;
            } 
            
            layoutGroup.spacing = new Vector2(InventoryParam.CELL_SPACING, InventoryParam.CELL_SPACING);
            cells = new UIItemDragCell[InventoryParam.MAX_EQUIPMENT_WIDTH, InventoryParam.MAX_EQUIPMENT_WIDTH];
            for (var i = 0; i < InventoryParam.MAX_EQUIPMENT_WIDTH; i++)
            {
                for (var j = 0; j < InventoryParam.MAX_EQUIPMENT_HEIGHT; j++)
                {
                    var cell = Instantiate(dragCellPrefab, layoutGroup.transform);
                    cell.SetActive(false);
                    
                    cells[i, j] = cell;
                }
            }

            isInitCell = true;
        }

        private void SetItemDragData(EquipmentData equipment)
        {
            var scaleSize = new Vector2(
                InventoryParam.CELL_SIZE * equipment.Width + (InventoryParam.CELL_SPACING * (equipment.Width - 1)), 
                InventoryParam.CELL_SIZE * equipment.Height + (InventoryParam.CELL_SPACING * (equipment.Height - 1)));
            
            myRect.sizeDelta = scaleSize;
            
            layoutGroup.cellSize = new Vector2(InventoryParam.CELL_SIZE, InventoryParam.CELL_SIZE);
            
            for (var i = 0; i < equipment.Width; i++)
            {
                for (var j = 0; j < equipment.Height; j++)
                {
                    cells[i, j].SetActive(true);
                }
            }
        }

        public void MarkItemInInventory(bool isIn)
        {
            isInInventory = isIn;
        }

        private void ActiveDragging()
        {
            transform.position = Mouse.current.position.ReadValue();
            
            EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ UIItem = this});
            
            isHoldingItem = true;
            delayFrameToUpdateHoverEvent = 0;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInInventory)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnPickingEquipmentFromInventory()
                {
                    UIItemPick = this,
                });
                
                MarkItemInInventory(false);
            }

            ActiveDragging();
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            isHoldingItem = false;
            
            EventManager.Instance.TriggerEvent(new EventData.OnPlacingEquipment()
            {
                UIItem = this
            });
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            delayFrameToUpdateHoverEvent = 0;
            transform.position = Mouse.current.position.ReadValue();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Mouse.current.position.ReadValue();
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 5 == 0)
            {
                EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ UIItem = this});
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }   
}
