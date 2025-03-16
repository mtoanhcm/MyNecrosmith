using Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace Minion.Inventory.UI
{
    public class UIMinionInventoryView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup cellParent;
        
        private Inventory currentInventory;
        private MinionInventoryViewComp inventoryViewComp;
        private bool isInit;
        
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

        public void CloseMinionInventory()
        {
            inventoryViewComp.ResetAllCellHoverState();
            ClearInventory();
        }

        private void OnCreateItemForMinionInventory(InventoryItem item)
        {
            
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
            
            void CreateUIInventoryComponent()
            {
                
            }
        }
        
        private void ClearInventory()
        {
            
        }
    }   
}
