using System;
using Equipment;
using Minion.Inventory.UI;
using Observer;
using Player.Inventory.UI;
using UnityEngine;

namespace Inventory.UI
{
    public class InventoryItemDragHandle : MonoBehaviour
    {
        [SerializeField] private UIMinionInventoryView minionInventoryView;
        [SerializeField] private UIPlayerInventoryView playerInventoryView;
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

        public void ResetDraggedItem(out EquipmentData currentDraggedEquipmentData)
        {
            currentDraggedEquipmentData = null; 
            if (currentDraggedItem == null)
            {
                return;
            }

            currentDraggedEquipmentData = currentDraggedItem.InventoryItem.Equipment;
            Destroy(currentDraggedItem.gameObject);
        }
        
        private void PickEquipmentInInventory(EventData.OnPickEquipmentInInventoryUI data)
        {
            if (data.Equipment != null)
            {
                CreateDraggingItem(data.Equipment);
                return;
            }
            
            currentDraggedItem.SetHoldingItem(false);
            currentDraggedItem.OnReleaseItemAction?.Invoke(currentDraggedItem);
        }

        private void CreateDraggingItem(EquipmentData data)
        {
            currentDraggedItem = Instantiate(draggedItemPrefab, transform);
            currentDraggedItem.Init(data);
            currentDraggedItem.SetHoldingItem(true);
            
            currentDraggedItem.OnCheckItemHover = CheckDraggingUIItem;
            currentDraggedItem.OnReleaseItemAction = CheckReleaseUIItem;
            currentDraggedItem.OnPickItemAction += CheckPickUIItem;
        }

        private void CheckDraggingUIItem(UIInventoryItem uiItem)
        {
            minionInventoryView.OnCheckDraggingEquipmentHoverInventory(uiItem);
        }

        private void CheckPickUIItem(UIInventoryItem uiItem)
        {
            minionInventoryView.RemoveItemFromInventory(uiItem);
        }
        
        private void CheckReleaseUIItem(UIInventoryItem uiItem)
        {
            uiItem.SetHoldingItem(false);
            currentDraggedItem = null;
            if (minionInventoryView.TryToPlaceUIInventoryItemToInventoryView(uiItem))
            {
                return;
            }

            if (playerInventoryView.TryToCheckOverlapAndReturnUIItemToInventory(uiItem))
            {
                Destroy(uiItem.gameObject);
                return;
            }
            
            if (minionInventoryView.IsHoldingUIItem(uiItem.GetInstanceID().ToString()))
            {
                minionInventoryView.PlaceUIItemBackToInventory(uiItem);
            }
            else
            {
                playerInventoryView.PlaceUIItemBackToInventory(uiItem);
                Destroy(uiItem.gameObject);
            }
        }
    }   
}
