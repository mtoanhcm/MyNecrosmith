using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Observer;
using UnityEngine;

namespace UI
{
    public class UIInventoryView : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPanel inventoryPanel;

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
        }
    }   
}
