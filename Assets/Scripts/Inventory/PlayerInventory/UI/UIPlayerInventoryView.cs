using System;
using System.Collections.Generic;
using Config;
using Equipment;
using Gameplay;
using GameUtility;
using Inventory.UI;
using Observer;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory.UI
{
    public class UIPlayerInventoryView : MonoBehaviour
    {
        [SerializeField]
        private GridLayoutGroup cellParent;
        [SerializeField] 
        private RectTransform inventoryRect;
        private List<UIPlayerInventoryCell> cells;

        private PlayerInventoryViewComponent playerInventoryComp;
        private int currentPage;

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OnPickEquipmentInInventoryUI>(OnPickEquipment);
        }

        private void OnDisable()
        {
            EventManager.Instance.StopListening<EventData.OnPickEquipmentInInventoryUI>(OnPickEquipment);
        }

        public void OpenPlayerInventory(PlayerInventory playerInventory)
        {
            if (cells == null || cells.Count == 0)
            {
                InitCell();
                currentPage = 0;
            }

            SetPlayerInventoryDataView(playerInventory);
        }

        public void ClosePlayerInventory()
        {
            
        }
        
        private void SetPlayerInventoryDataView(PlayerInventory playerInventory)
        {
            if (playerInventoryComp == null)
            {
                playerInventoryComp = new PlayerInventoryViewComponent(playerInventory, cells.Count);
            }

            var equipmentList = playerInventoryComp.GetEquipmentPage(currentPage, EquipmentCategoryID.None);
            for (var i = 0; i < equipmentList.Length; i++)
            {
                var equipment = equipmentList[i];
                if (equipment == null)
                {
                    cells[i].SetEquipmentData(null, 0);
                    continue;
                }
                
                cells[i].SetEquipmentData(equipment.data, equipment.Amount);
            }
        }
        
        private void OnPickEquipment(EventData.OnPickEquipmentInInventoryUI data)
        {
            if (playerInventoryComp.PlayerInventory.RemoveEquipment(data.Equipment))
            {
                SetPlayerInventoryDataView(playerInventoryComp.PlayerInventory);
            }
        }
        
        private void InitCell()
        {
            cells = new List<UIPlayerInventoryCell>(cellParent.gameObject.GetComponentsInChildren<UIPlayerInventoryCell>());
            for (var i = 0; i < cells.Count; i++)
            {
                cells[i].Init();
            }
        }

        public bool TryToCheckOverlapAndReturnUIItemToInventory(UIInventoryItem uiItem)
        {
            if (!uiItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return false;
            }

            PlaceUIItemBackToInventory(uiItem);

            return true;
        }

        public void PlaceUIItemBackToInventory(UIInventoryItem uiItem)
        {
            playerInventoryComp.PlayerInventory.AddEquipmentToStorage(uiItem.InventoryItem.Equipment);
            SetPlayerInventoryDataView(playerInventoryComp.PlayerInventory);
        }
    }   
}
