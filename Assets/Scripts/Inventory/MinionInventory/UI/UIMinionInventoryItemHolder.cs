using System.Collections.Generic;
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
    }   
}
