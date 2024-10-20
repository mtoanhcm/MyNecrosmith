using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Character;
using Equipment;
using Observer;
using Ultility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace  UI
{
    public class UIItemDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public Action<EquipmentData> OnDragEquipment;
        public RectTransform MyRect => myRect;
        public UIItemDragCell[,] Cells => cells;

        [SerializeField] private GridLayoutGroup layoutGroup;
        [SerializeField] private UIItemDragCell dragCellPrefab;
        [SerializeField] private Image iconImg;
        private EquipmentData equipment;
        private RectTransform myRect;

        private int delayFrameToUpdateHoverEvent;
        private UIItemDragCell[,] cells;

        private bool isInit;
        
        private void Awake()
        {   
            //Set size UI
            myRect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            SetItemDragData(new EquipmentData(){ Width = 1, Height = 2, IconSpr = iconImg.sprite });
        }

        public void SetItemDragData(EquipmentData data)
        {
            if (!isInit)
            {
                Init();
            }
            
            equipment = data;
            var scaleSize = new Vector2(
                InventoryParam.CELL_SIZE * equipment.Width + (layoutGroup.spacing.x * (equipment.Width - 1)), 
                InventoryParam.CELL_SIZE * equipment.Height + (layoutGroup.spacing.y * (equipment.Height - 1)));
            
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
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            delayFrameToUpdateHoverEvent = 0;
            transform.position = Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 10 == 0)
            {
                EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ DragItem = this});
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EventManager.Instance.TriggerEvent(new EventData.OnPlacingEquipment()
            {
                Equipment = equipment,
                EquipementRect = myRect,
            });
        }

        private void Init()
        {
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

            isInit = true;
        }
    }   
}
