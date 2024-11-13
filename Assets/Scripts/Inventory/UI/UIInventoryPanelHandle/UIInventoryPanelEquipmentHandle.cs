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

        public List<EquipmentData> GetEquipmentData()
        {
            var equipmentData = new List<EquipmentData>();
            for (var i = 0; i < inventoryItems.Count; i++)
            {
                equipmentData.Add(inventoryItems[i].Item.Equipment);
            }
            
            return equipmentData;
        }
    }   
}
