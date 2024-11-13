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

        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.OpenCharacterInventory>(OnGetCharacterInventoryData);
        }

        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OpenCharacterInventory>(OnGetCharacterInventoryData);
        }
        
        private void OnGetCharacterInventoryData(EventData.OpenCharacterInventory data)
        {
            inventoryPanel.OpenInventory(data.InventoryData);
            
            inventoryPanel.SetActive(data.InventoryData != null);
            grandStore.SetActive(data.InventoryData != null);
        }
    }   
}
