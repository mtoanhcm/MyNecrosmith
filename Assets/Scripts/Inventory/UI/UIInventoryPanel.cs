using System.Linq;
using Observer;
using UnityEngine;
using Character;
using Config;
using GameUtility;
using UnityEngine.UI;

namespace UI
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup cellGridContainer;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private UIInventoryCell cellPrefab;
        [SerializeField] private Button spawnCharacterBtn;
        
        private RectTransform inventoryRect;

        private CharacterClass characterClassOwnInventory;
        private UIInventoryPanelCellHandle cellHandle;
        private UIInventoryPanelEquipmentHandle equipmentHandle;

        private bool isInit;
        
        private void Init()
        {
            //InitInventorySize();
            InitInventoryEmptyCell();

            equipmentHandle = new UIInventoryPanelEquipmentHandle();   
            inventoryRect = cellGridContainer.GetComponent<RectTransform>();
            
            spawnCharacterBtn.onClick.RemoveAllListeners();
            spawnCharacterBtn.onClick.AddListener(OnCharacterEquipmentReady);
            
            return;

            // void InitInventorySize()
            // {
            //     inventoryRect = cellGridContainer.GetComponent<RectTransform>();
            //     inventoryRect.sizeDelta = new Vector2(
            //         InventoryParam.CELL_SIZE * InventoryParam.MAX_COLUMN +
            //         (cellGridContainer.spacing.x * (InventoryParam.MAX_COLUMN - 1)),
            //         InventoryParam.CELL_SIZE * InventoryParam.MAX_ROW +
            //         (cellGridContainer.spacing.x * (InventoryParam.MAX_ROW - 1))
            //     );
            //
            //     cellGridContainer.spacing = new Vector2(InventoryParam.CELL_SPACING, InventoryParam.CELL_SPACING);
            //     cellGridContainer.cellSize = new Vector2(InventoryParam.CELL_SIZE, InventoryParam.CELL_SIZE);
            // }

            void InitInventoryEmptyCell()
            {
                cellHandle = new UIInventoryPanelCellHandle(InventoryParam.MAX_ROW, InventoryParam.MAX_COLUMN);
                var allCellObj = cellGridContainer.transform.GetComponentsInChildren<UIInventoryCell>(true);

                if (cellHandle.InventoryCellHash.Count != allCellObj.Length)
                {
                    Debug.LogError("The total cells in inventory data do not match");
                    return;
                }

                var index = 0;
                foreach (var pos in cellHandle.InventoryCellHash)
                {
                    var cell = allCellObj[index];
                    cell.name = pos.ToString();
                    cell.Init(pos.Item1, pos.Item2);
                    
                    cellHandle.SetUIInventoryCell(pos.Item1, pos.Item2, cell);

                    index++;
                }
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
            EventManager.Instance.StartListening<EventData.OnPlacingEquipment>(OnPlaceEquipmentToInventory);
            EventManager.Instance.StartListening<EventData.OnPickingEquipmentFromInventory>(OnPickingEquipmentFromInventory);
        }

        private void OnDisable()
        {
            cellHandle.LockAllCells();
            EventManager.Instance?.StopListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
            EventManager.Instance?.StopListening<EventData.OnPlacingEquipment>(OnPlaceEquipmentToInventory);
            EventManager.Instance?.StopListening<EventData.OnPickingEquipmentFromInventory>(OnPickingEquipmentFromInventory);
        }

        public void OpenInventory(Inventory characterInventory)
        {
            if (!isInit)
            {
                Init();
                isInit = true;
            }

            if (characterInventory == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            characterClassOwnInventory = characterInventory.CharacterClass;
            
            cellHandle.LockAllCells();
            cellHandle.ResetAllCellHoverState();

            SetVisibleInventoryCell();
            
            equipmentHandle.SetInventoryItems(null);
            
            return;
            
            void SetVisibleInventoryCell()
            {
                for (var i = 0; i < characterInventory.Row; i++)
                {
                    for (var j = 0; j < characterInventory.Column; j++)
                    {
                        var posX = InventoryParam.MAX_ROW / 2 +  (i - characterInventory.Row / 2);
                        var posY = InventoryParam.MAX_COLUMN / 2 + (j - characterInventory.Column / 2);
                        cellHandle.SetLockCell(posX, posY, false);
                    }
                }
            }
        }

        private void OnCharacterEquipmentReady()
        {
            var config = Resources.Load<MinionConfig>($"Character/{characterClassOwnInventory}");
            if (config != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnSpawnMinion()
                {
                    Config = config,
                    Equipments = equipmentHandle.GetEquipmentData(),
                    SpawnPosition = Vector3.zero
                });

                CloseInventoryUIPanel();
            }
        }
        
        private void OnCheckDraggingEquipmentHoverInventory(EventData.DraggingEquipment data)
        {
            if (!data.UIItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return;
            }
            
            cellHandle.ResetAllCellHoverState();
            cellHandle.CheckHoverCell(data.UIItem, inventoryRect);
        }
        
        private void OnPlaceEquipmentToInventory(EventData.OnPlacingEquipment data)
        {
            if (!cellHandle.CanPlaceEquipmentOnCells(data.UIItem, inventoryRect, out var claimPos))
            {
                return;
            }
            
            data.UIItem.Item.UpdatePosInInventory(claimPos);
            data.UIItem.MarkItemInInventory(true);
            data.UIItem.transform.SetParent(itemContainer);
            
            var uiItemClaimPos = data.UIItem.Item.PosClaimInventory.First();
            data.UIItem.transform.position = cellHandle.GetCenterPositionOfCellArea(uiItemClaimPos.Item1,
                uiItemClaimPos.Item2, data.UIItem.Item.Equipment.Width, data.UIItem.Item.Equipment.Height);
            
            cellHandle.SetItemForCell(claimPos, data.UIItem.GetInstanceID().ToString());
            cellHandle.ResetAllCellHoverState();
            equipmentHandle.AddItemToInventory(data.UIItem);
        }

        private void OnPickingEquipmentFromInventory(EventData.OnPickingEquipmentFromInventory data)
        {
            cellHandle.RemoveItemForcell(data.UIItemPick.GetInstanceID().ToString());
        }

        private void CloseInventoryUIPanel()
        {
            EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory() { InventoryData = null });
        }
    }   
}
