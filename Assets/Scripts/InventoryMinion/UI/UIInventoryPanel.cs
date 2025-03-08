using System.Linq;
using Observer;
using UnityEngine;
using Character;
using Config;
using GameUtility;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Manages the inventory panel UI and its interactions.
    /// Coordinates between the inventory data and the UI representations.
    /// </summary>
    public class UIInventoryPanel : MonoBehaviour
    {
        /// <summary>
        /// Grid layout for containing the inventory cells
        /// </summary>
        [SerializeField] private GridLayoutGroup cellGridContainer;
        
        /// <summary>
        /// Container for inventory items
        /// </summary>
        [SerializeField] private Transform itemContainer;
        
        /// <summary>
        /// Prefab for inventory cells
        /// </summary>
        [SerializeField] private UIInventoryCell cellPrefab;
        
        /// <summary>
        /// Button to spawn a character with the current equipment
        /// </summary>
        [SerializeField] private Button spawnCharacterBtn;
        
        /// <summary>
        /// The RectTransform of the inventory panel
        /// </summary>
        private RectTransform inventoryRect;
        
        /// <summary>
        /// ID of the character that owns this inventory
        /// </summary>
        private CharacterID _characterIDOwnInventory;
        
        /// <summary>
        /// Handler for cell operations
        /// </summary>
        private UIInventoryPanelCellHandle cellHandle;
        
        /// <summary>
        /// Handler for equipment operations
        /// </summary>
        private UIInventoryPanelEquipmentHandle equipmentHandle;
        
        /// <summary>
        /// Whether the panel has been initialized
        /// </summary>
        private bool isInit;
        
        /// <summary>
        /// Initializes the inventory panel
        /// </summary>
        private void Init()
        {
            InitInventoryEmptyCell();

            equipmentHandle = new UIInventoryPanelEquipmentHandle();   
            inventoryRect = cellGridContainer.GetComponent<RectTransform>();
            
            spawnCharacterBtn.onClick.RemoveAllListeners();
            spawnCharacterBtn.onClick.AddListener(OnCharacterEquipmentReady);
        }
        
        /// <summary>
        /// Initializes the inventory cells
        /// </summary>
        private void InitInventoryEmptyCell()
        {
            cellHandle = new UIInventoryPanelCellHandle(InventoryConstants.MAX_ROW, InventoryConstants.MAX_COLUMN);
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
        
        /// <summary>
        /// Called when the inventory panel is enabled
        /// </summary>
        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
            EventManager.Instance.StartListening<EventData.OnPlacingEquipment>(OnPlaceEquipmentToInventory);
            EventManager.Instance.StartListening<EventData.OnPickingEquipmentFromInventory>(OnPickingEquipmentFromInventory);
        }
        
        /// <summary>
        /// Called when the inventory panel is disabled
        /// </summary>
        private void OnDisable()
        {
            if (cellHandle != null)
                cellHandle.LockAllCells();
                
            EventManager.Instance?.StopListening<EventData.DraggingEquipment>(OnCheckDraggingEquipmentHoverInventory);
            EventManager.Instance?.StopListening<EventData.OnPlacingEquipment>(OnPlaceEquipmentToInventory);
            EventManager.Instance?.StopListening<EventData.OnPickingEquipmentFromInventory>(OnPickingEquipmentFromInventory);
        }
        
        /// <summary>
        /// Opens the inventory panel for the specified character
        /// </summary>
        /// <param name="characterInventory">The inventory data to display</param>
        public void OpenInventory(InventoryData characterInventory)
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
            
            _characterIDOwnInventory = characterInventory.CharacterID;
            
            cellHandle.LockAllCells();
            cellHandle.ResetAllCellHoverState();

            // Set visible inventory cells based on character's inventory size
            SetVisibleInventoryCell(characterInventory.Row, characterInventory.Column);
            
            // Clear equipment handle
            equipmentHandle.SetInventoryItems(null);
            
            // Load existing items if any
            if (characterInventory.Items.Count > 0)
            {
                foreach (var item in characterInventory.Items)
                {
                    AddItemToInventory(item);
                }
            }
        }
        
        /// <summary>
        /// Sets which cells are visible based on the inventory dimensions
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        private void SetVisibleInventoryCell(int rows, int columns)
        {
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var posX = InventoryConstants.MAX_ROW / 2 +  (i - rows / 2);
                    var posY = InventoryConstants.MAX_COLUMN / 2 + (j - columns / 2);
                    cellHandle.SetLockCell(posX, posY, false);
                }
            }
        }
        
        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        private void AddItemToInventory(InventoryItem item)
        {
            // Create a UI item
            var uiItem = Instantiate(cellPrefab, itemContainer).GetComponent<UIInventoryItem>();
            uiItem.Init(item.Equipment);
            uiItem.MarkItemInInventory(true);
            
            // Set its position based on the item's inventory position
            var firstPos = item.PosClaimInventory.First();
            uiItem.transform.position = cellHandle.GetCenterPositionOfCellArea(
                firstPos.Item1, firstPos.Item2, item.Width, item.Height);
            
            // Mark the cells as occupied by this item
            cellHandle.SetItemForCell(item.PosClaimInventory, uiItem.GetInstanceID().ToString());
            
            // Add the item to the equipment handle
            equipmentHandle.AddItemToInventory(uiItem);
        }
        
        /// <summary>
        /// Called when the spawn character button is clicked
        /// </summary>
        private void OnCharacterEquipmentReady()
        {
            var config = Resources.Load<MinionConfig>($"Character/Minion/{_characterIDOwnInventory}");
            if (config != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnPrepareEquipmentForSpawnMinion()
                {
                    MinionConfig = config,
                    Equipment = equipmentHandle.GetEquipmentData(),
                });

                CloseInventoryUIPanel();
            }
            else
            {
                Debug.LogError($"Cannot find the character config for {_characterIDOwnInventory}");
            }
        }
        
        /// <summary>
        /// Called when an item is being dragged over the inventory
        /// </summary>
        /// <param name="data">Dragging event data</param>
        private void OnCheckDraggingEquipmentHoverInventory(EventData.DraggingEquipment data)
        {
            if (!data.UIItem.MyRect.IsWorldOverlap(inventoryRect))
            {
                return;
            }
            
            cellHandle.ResetAllCellHoverState();
            cellHandle.CheckHoverCell(data.UIItem, inventoryRect);
        }
        
        /// <summary>
        /// Called when an item is being placed in the inventory
        /// </summary>
        /// <param name="data">Placing event data</param>
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
            
            data.OnPlaceEquipmentInInventorySuccess?.Invoke(data.UIItem.Item.Equipment.EquipmentID);
        }
        
        /// <summary>
        /// Called when an item is being picked up from the inventory
        /// </summary>
        /// <param name="data">Picking event data</param>
        private void OnPickingEquipmentFromInventory(EventData.OnPickingEquipmentFromInventory data)
        {
            cellHandle.RemoveItemForcell(data.UIItemPick.GetInstanceID().ToString());
            equipmentHandle.RemoveItemFromInventory(data.UIItemPick);
        }
        
        /// <summary>
        /// Closes the inventory panel
        /// </summary>
        private void CloseInventoryUIPanel()
        {
            EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory() { InventoryData = null });
        }
    }
}