using System;
using Equipment;
using GameUtility;
using Observer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIEquipmentStorageCell : MonoBehaviour, IPointerClickHandler
    {
        public bool HasEquipment {get; private set;}
        
        [SerializeField] private Image equipmentIconImg;
        
        private EquipmentData currentEquipment;

        private void Awake()
        {
            equipmentIconImg.SetActive(false);
        }

        public void SetData(EquipmentData equipment)
        {
            HasEquipment = equipment != null;
            currentEquipment = equipment;
            equipmentIconImg.SetActive(HasEquipment);
            equipmentIconImg.sprite = HasEquipment ? equipment.IconSpr : null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!HasEquipment)
            {
                return;
            }
            
            EventManager.Instance.TriggerEvent(new EventData.OnChooseEquipmentInStorage() { Equipment = currentEquipment });
        }
    }   
}
