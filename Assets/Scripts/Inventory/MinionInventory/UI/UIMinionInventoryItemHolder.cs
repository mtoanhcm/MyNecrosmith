using System.Collections.Generic;
using Equipment;
using Inventory.UI;
using UnityEngine;

namespace Minion.Inventory.UI
{
    public class UIMinionInventoryItemHolder : MonoBehaviour
    {
        private Dictionary<string, UIInventoryItem> itemDic;

        public void Init()
        {
            itemDic = new Dictionary<string, UIInventoryItem>();
        }

        public EquipmentData[] GetEquipmentData()
        {
            var tempData = new EquipmentData[itemDic.Count];
            if (tempData.Length == 0)
            {
                return tempData;
            }

            var index = 0;
            foreach (var item in itemDic)
            {
                tempData[index] = item.Value.InventoryItem.Equipment;
                index++;
            }
            
            return tempData;
        }

        public void AddUIItemToInventory(UIInventoryItem item)
        {
            if (itemDic.ContainsKey(item.GetInstanceID().ToString()))
            {
                return;
            }
            
            itemDic[item.GetInstanceID().ToString()] = item;
            item.transform.SetParent(transform);
        }

        public void RemoveUIItemFromInventory(string itemID)
        {
            if (!itemDic.ContainsKey(itemID))
            {
                return;
            }
            
            itemDic.Remove(itemID);
            
            //Destroy(item.gameObject);
        }

        public bool IsHoldingUIItem(string itemID)
        {
            return itemDic.ContainsKey(itemID);
        }

        public EquipmentData[] ClearAllHolderItems()
        {
            var holderEquipmentData = new List<EquipmentData>();
            foreach (var item in itemDic)
            {
                holderEquipmentData.Add(item.Value.InventoryItem.Equipment);
                Destroy(item.Value.gameObject);
            }
            
            itemDic.Clear();
            
            return holderEquipmentData.ToArray();
        }
    }   
}
