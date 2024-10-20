using System.Collections.Generic;
using Character;
using Equipment;
using UnityEngine;

namespace UI
{
    public class UIInventoryPanelEquipmentHandle
    {
        private List<UIInventoryItem> inventoryItems;

        public UIInventoryPanelEquipmentHandle()
        {
            inventoryItems = new List<UIInventoryItem>();
        }

        public void SetInventoryItems(List<UIInventoryItem> inventoryItems)
        {
            this.inventoryItems.Clear();

            if (inventoryItems != null)
            {
                this.inventoryItems = inventoryItems;   
            }
        }

        public void AddItemToInventory(UIInventoryItem item)
        {
            inventoryItems.Add(item);
        }
    }   
}
