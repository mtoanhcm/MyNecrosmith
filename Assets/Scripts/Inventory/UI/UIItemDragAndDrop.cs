using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Equipment;
using Observer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace  UI
{
    public class UIItemDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public Action<EquipmentData> OnDragEquipment;

        [SerializeField] private GridLayoutGroup layoutGroup;
        private EquipmentData equipment;
        private RectTransform myRect;

        private int delayFrameToUpdateHoverEvent;
        
        private void Awake()
        {   
            equipment = new EquipmentData()
            {
                Width = 1,
                Height = 2
            };
            
            //Set size UI
            myRect = GetComponent<RectTransform>();
            myRect.sizeDelta = new Vector2(
                InventoryParam.CELL_SIZE * equipment.Width + (layoutGroup.spacing.x * (equipment.Width - 1)), 
                InventoryParam.CELL_SIZE * equipment.Height + (layoutGroup.spacing.y * (equipment.Height - 1)));
            layoutGroup.cellSize = new Vector2(InventoryParam.CELL_SIZE, InventoryParam.CELL_SIZE);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            delayFrameToUpdateHoverEvent = 0;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 10 == 0)
            {
                EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ Data = equipment, ItemDragRec = myRect});
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }   
}
