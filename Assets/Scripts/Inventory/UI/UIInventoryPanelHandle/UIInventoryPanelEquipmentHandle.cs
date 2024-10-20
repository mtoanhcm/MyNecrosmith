using System.Collections.Generic;
using Character;
using Equipment;
using UnityEngine;

namespace UI
{
    public class UIInventoryPanelEquipmentHandle
    {
        private List<InventoryItem> inventoryItems;

        public UIInventoryPanelEquipmentHandle()
        {
            inventoryItems = new List<InventoryItem>();
        }

        public void SetInventoryItems(List<InventoryItem> inventoryItems)
        {
            inventoryItems.Clear();
            inventoryItems.AddRange(inventoryItems);
        }
    }   
}
