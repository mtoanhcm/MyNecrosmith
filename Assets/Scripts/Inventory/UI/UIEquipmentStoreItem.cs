using System;
using System.Collections.Generic;
using Character;
using Config;
using Equipment;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIEquipmentStoreItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image equipmentIcon;
        [SerializeField] private Transform slotContainer;
        
        private List<GameObject> slots;

        private const float SLOT_WIDTH = 12f;
        private const float SLOT_HEIGHT = 12f;
        private const float SLOT_SPACING = 2f;

        private void InitDefaultSlots()
        {
            slots = new List<GameObject>();
            var totalSlots = InventoryParam.MAX_EQUIPMENT_WIDTH * InventoryParam.MAX_EQUIPMENT_HEIGHT;
            for (var i = 0; i < totalSlots; i++)
            {
                var obj = new GameObject($"slot_{i}", typeof(Image));
                obj.transform.SetParent(slotContainer);
                
                var rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(SLOT_WIDTH, SLOT_HEIGHT);
                
                var image = obj.GetComponent<Image>();
                image.color = Color.green;
                
                obj.SetActive(false);
                
                slots.Add(obj);
            }
        }

        public void SetData(EquipmentData data)
        {
            equipmentIcon.sprite = data.IconSpr;

            if (slots == null)
            {
                InitDefaultSlots();
            }
            
            HideAllSlots();
            var index = 0;
            for (var i = 0; i < data.Width; i++)
            {
                for (var j = 0; j < data.Height; j++)
                {
                    var slot = slots[index];
                    slot.transform.position = new Vector3(slotContainer.position.x + i * SLOT_WIDTH + (i * SLOT_SPACING), slotContainer.position.y - j * SLOT_HEIGHT - (j * SLOT_SPACING), 0);
                    slot.SetActive(true);
                    
                    index++;
                }
            }
        }

        private void HideAllSlots()
        {
            for (var i = 0; i < slots.Count; i++)
            {
                slots[i].SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
        }
    }   
}
