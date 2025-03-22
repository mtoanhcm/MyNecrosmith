using System;
using Equipment;
using GameUtility;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory.UI
{
    public class UIPlayerInventoryCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool HasEquipment => equipmentData != null;
        
        [SerializeField] private Image equipmentIconImg;
        [SerializeField] private TextMeshProUGUI amountTxt;
        
        private EquipmentData equipmentData;
        
        public void Init()
        {
            equipmentIconImg.SetActive(false);
            amountTxt.SetActive(false);
        }

        public void SetEquipmentData(EquipmentData data, int amount)
        {
            equipmentData = data;
            equipmentIconImg.SetActive(equipmentData != null);
            amountTxt.SetActive(amount > 0);
            if (equipmentData != null)
            {
                equipmentIconImg.sprite = equipmentData.IconSpr;
                amountTxt.text = $"x {amount}";
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!HasEquipment)
            {
                return;
            }
            
            EventManager.Instance.TriggerEvent(new EventData.OnPickEquipmentInInventoryUI(){ Equipment = equipmentData });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            EventManager.Instance.TriggerEvent(new EventData.OnPickEquipmentInInventoryUI(){ Equipment = null });
        }
    }   
}
