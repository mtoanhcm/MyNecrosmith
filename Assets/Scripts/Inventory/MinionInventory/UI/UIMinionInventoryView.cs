using System.Linq;
using Equipment;
using GameUtility;
using Inventory.UI;
using Observer;
using UnityEngine;
using UnityEngine.UI;

namespace Minion.Inventory.UI
{
    public class UIMinionInventoryView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup cellParent;
        [SerializeField] private RectTransform inventoryRect;
        [SerializeField] private UIMinionInventoryItemHolder inventoryItemHolder;
        
        private Inventory currentInventory;
        private MinionInventoryViewComp inventoryViewComp;
        private bool isInit;
        
        private void OnEnable()
        {
            //EventManager.Instance.StartListening<EventData.OnPlaceInventoryItemUI>(TryToPlaceUIInventoryItemToInventoryView);
        }

        private void OnDisable()
        {
            //EventManager.Instance.StopListening<EventData.OnPlaceInventoryItemUI>(TryToPlaceUIInventoryItemToInventoryView);
        }
        
        public void OpenMinionInventory(Inventory minionInventory)
        {
            currentInventory = minionInventory;
            
            if (!isInit)
            {
                InitInventory();
            }
            else
            {
                inventoryViewComp.LockAllCells();
            }

            SetupInventoryCellBaseOnMinionInventoryInfo(minionInventory);
        }

        public void ClearInventory(out EquipmentData[] holderEquipmentData)
        {
            inventoryViewComp.ClearInventoryCells();
            holderEquipmentData = inventoryItemHolder.ClearAllHolderItems();
        }

        private void OnCreateItemForMinionInventory(InventoryItem item)
        {
            
        }
        
        public bool TryToPlaceUIInventoryItemToInventoryView(UIInventoryItem uiItem)
        {
            if (!uiItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return false;
            }
            
            if (!inventoryViewComp.CanPlaceEquipmentOnCells(uiItem, inventoryRect, out var claimPos))
            {
                return false;
            }

            uiItem.InventoryItem.UpdatePosInInventory(claimPos);
            inventoryItemHolder.AddUIItemToInventory(uiItem);
            //uiItem.transform.SetParent(inventoryItemHolderTrans);

            PlaceUIItemBackToInventory(uiItem);
            //equipmentHandle.AddItemToInventory(data.UIItem);

            //data.OnPlaceItemInMinionInventorySuccess?.Invoke(data.UIItem.InventoryItem.Equipment);
            
            return true;
        }

        public void PlaceUIItemBackToInventory(UIInventoryItem uiItem)
        {
            var uiItemClaimPos = uiItem.InventoryItem.PosClaimInventory.First();
            uiItem.transform.position = inventoryViewComp.GetCenterPositionOfCellArea(uiItemClaimPos.Item1,
                uiItemClaimPos.Item2, uiItem.InventoryItem.Equipment.Width, uiItem.InventoryItem.Equipment.Height);

            inventoryViewComp.SetItemForCell(uiItem.InventoryItem.PosClaimInventory, uiItem.GetInstanceID().ToString());
            inventoryViewComp.ResetAllCellHoverState();
        }

        public bool IsHoldingUIItem(string itemID)
        {
            return inventoryItemHolder.IsHoldingUIItem(itemID);
        }

        public void RemoveItemFromInventory(UIInventoryItem item, bool isCompleteRemove = false)
        {
            var itemID = item.GetInstanceID().ToString();
            inventoryViewComp.RemoveItemForCell(itemID);

            if (isCompleteRemove)
            {
                inventoryItemHolder.RemoveUIItemFromInventory(itemID);
            }
        }
        
        public void OnCheckDraggingEquipmentHoverInventory(UIInventoryItem uiItem)
        {
            if (!uiItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return;
            }
    
            inventoryViewComp.ResetAllCellHoverState();
            inventoryViewComp.CheckHoverCell(uiItem, inventoryRect);
        }

        public bool TryToGetInventoryItemsForSpawnMinion(out EquipmentData[] equipmentData)
        {
            equipmentData = inventoryItemHolder.GetEquipmentData();
            return equipmentData.Length > 0;
        }
        
        private void SetupInventoryCellBaseOnMinionInventoryInfo(Inventory minionInventory)
        {
            for (var i = 0; i < minionInventory.Row; i++)
            {
                for (var j = 0; j < minionInventory.Column; j++)
                {
                    var indexX = MinionInventoryParam.MAX_ROW / 2 +  (i - minionInventory.Row / 2);
                    var indexY = MinionInventoryParam.MAX_COLUMN / 2 + (j - minionInventory.Column / 2);
                    inventoryViewComp.SetLockCell(indexX, indexY, false);
                }
            }
        }
        
        private void InitInventory()
        {
            InitInventoryCell();
            inventoryItemHolder.Init();
            
            isInit = true;

            return;

            void InitInventoryCell()
            {
                inventoryViewComp = new MinionInventoryViewComp(MinionInventoryParam.MAX_ROW, MinionInventoryParam.MAX_COLUMN);
                var allCellObj = cellParent.transform.GetComponentsInChildren<UIMinionInventoryCell>(true);

                if (inventoryViewComp.InventoryCellHash.Count != allCellObj.Length)
                {
                    Debug.LogError("The total cells in inventory data do not match");
                    return;
                }
                
                var index = 0;
                foreach (var pos in inventoryViewComp.InventoryCellHash)
                {
                    var cell = allCellObj[index];
                    cell.name = pos.ToString();
                    cell.Init(pos.Item1, pos.Item2);
    
                    inventoryViewComp.SetUIInventoryCell(pos.Item1, pos.Item2, cell);

                    index++;
                }
            }
        }
    }   
}
