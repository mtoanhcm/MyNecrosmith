using System;
using Equipment;
using Observer;
using UnityEngine;

namespace Inventory.UI
{
    public class InventoryItemDragHandle : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem draggedItemPrefab;
        
        private UIInventoryItem currentDraggedItem;
        
        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OnPickEquipmentInInventoryUI>(PickEquipmentInInventory);
        }

        private void OnDisable()
        {
            EventManager.Instance.StopListening<EventData.OnPickEquipmentInInventoryUI>(PickEquipmentInInventory);
        }

        private void PickEquipmentInInventory(EventData.OnPickEquipmentInInventoryUI data)
        {
            if (data.Equipment != null)
            {
                CreateDraggingItem(data.Equipment);
                return;
            }
            
            currentDraggedItem.SetHoldingItem(false);
            
        }

        private void CreateDraggingItem(EquipmentData data)
        {
            currentDraggedItem = Instantiate(draggedItemPrefab, transform);
            currentDraggedItem.Init(data);
            currentDraggedItem.SetHoldingItem(true);
        }
    }   
}
