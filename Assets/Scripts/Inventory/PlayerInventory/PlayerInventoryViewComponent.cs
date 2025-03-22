using System.Collections.Generic;
using System.Linq;
using Config;
using Equipment;
using Gameplay;
using Minion.Inventory;
using UnityEngine;

namespace Player.Inventory
{
    public class PlayerInventoryViewComponent
    {
        public class EquipmentSlotData
        {
            public EquipmentData data;
            public int Amount;
        }
        
        public PlayerInventory PlayerInventory => playerInventory;
        
        private PlayerInventory playerInventory;

        private Dictionary<int, EquipmentSlotData[]> equipmentPages;
        private int totalItemInPage;
        
        public PlayerInventoryViewComponent(PlayerInventory inventory, int maxEquipmentEachPage)
        {
            playerInventory = inventory;
            totalItemInPage = maxEquipmentEachPage;
            equipmentPages = new Dictionary<int, EquipmentSlotData[]>();
        }

        public EquipmentSlotData[] GetEquipmentPage(int pageIndex, EquipmentCategoryID category)
        {
            var equipmentSlotData = new EquipmentSlotData[totalItemInPage];
            var allEquipmentByCategory = playerInventory.GetEquipmentsByCategory(category);
            
            var startIndex = pageIndex * totalItemInPage;
            if (startIndex > allEquipmentByCategory.Count)
            {
                return equipmentSlotData;
            }

            var index = 0;
            var keyList = allEquipmentByCategory.Keys.ToList();
            for (var i = startIndex; i < keyList.Count; i++)
            {
                equipmentSlotData[index] = new EquipmentSlotData()
                {
                    data = allEquipmentByCategory[keyList[i]].First(),
                    Amount = allEquipmentByCategory[keyList[i]].Count
                };

                index++;
                if (index > equipmentSlotData.Length - 1)
                {
                    break;
                }
            }
            
            return equipmentSlotData;
        }
    }   
}
