using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using GameUtility;
using Observer;
using UnityEngine;

namespace UI
{
    public class UIInventoryView : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPanel inventoryPanel;
        [SerializeField]
        private UIEquipmentGrandStore grandStore;
        
        public void OpenCharacterInventory(Inventory data)
        {
            gameObject.SetActive(data != null);
            if (data == null)
            {
                return;
            }
            
            inventoryPanel.OpenInventory(data);
        }
    }   
}
