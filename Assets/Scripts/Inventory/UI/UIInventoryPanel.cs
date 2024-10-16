using System;
using Observer;
using UnityEngine;
using Character;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [SerializeField] private Transform cellContainer;
        [SerializeField] private UIInventoryCell cellPrefab;

        private UIInventoryCell[,] cells;

        private void Awake()
        {
            cells = new UIInventoryCell[InventoryParam.MAX_ROW, InventoryParam.MAX_COLUMN];
            for (var i = 0; i < InventoryParam.MAX_ROW; i++)
            {
                for (var j = 0; j < InventoryParam.MAX_COLUMN; j++)
                {
                    var cell = Instantiate(cellPrefab, cellContainer).GetComponent<UIInventoryCell>();
                    cell.Init(i,j);
                    
                    cells[i,j] = cell;
                }
            }
        }

        private void OnDisable()
        {
            LockAllCells();
        }

        public void OpenInventory(Inventory characterInventory)
        {
            LockAllCells();
            
            for (var i = 0; i < characterInventory.Row; i++)
            {
                for (var j = 0; j < characterInventory.Column; j++)
                {
                    var posX = InventoryParam.MAX_ROW / 2 +  (i - characterInventory.Row / 2);
                    var posY = InventoryParam.MAX_COLUMN / 2 + (j - characterInventory.Column / 2);
                    
                    cells[posX, posY].SetLockCell(false);
                }
            }
        }
        
        private void LockAllCells()
        {
            for (var i = 0; i < InventoryParam.MAX_ROW; i++)
            {
                for (var j = 0; j < InventoryParam.MAX_COLUMN; j++)
                {
                    cells[i,j].SetLockCell(true);
                }
            }
        }
    }   
}
