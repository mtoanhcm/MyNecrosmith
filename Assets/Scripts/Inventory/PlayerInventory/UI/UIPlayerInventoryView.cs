using System;
using System.Collections.Generic;
using Config;
using Equipment;
using Gameplay;
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
        private List<UIPlayerInventoryCell> cells;

        private PlayerInventoryViewComponent playerInventoryComp;

        public void OpenPlayerInventory(PlayerInventory playerInventory)
        {
            if (cells == null || cells.Count == 0)
            {
                InitCell();
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

            var equipmentList = playerInventoryComp.GetEquipmentPage(0, EquipmentCategoryID.Sword);
            for (var i = 0; i < equipmentList.Length; i++)
            {
                var equipment = equipmentList[i];
                if (equipment == null)
                {
                    continue;
                }
                
                cells[i].SetEquipmentData(equipment.data, equipment.Amount);
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
    }   
}
