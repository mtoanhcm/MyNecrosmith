using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Observer;
using UnityEngine.InputSystem;

namespace Inventory.UI
{
    /// <summary>
    /// Represents an item in the inventory UI.
    /// Handles visual representation and drag-and-drop functionality for inventory items.
    /// </summary>
    public class UIInventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, 
                                   IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// The rect transform of this item
        /// </summary>
        public RectTransform MyRect => myRect;
        
        /// <summary>
        /// The inventory item data this UI item represents
        /// </summary>
        public InventoryItem Item { get; private set; }
        
        /// <summary>
        /// The 2D array of drag cells used for visualization
        /// </summary>
        public UIItemDragCell[,] Cells => cells;
        
        /// <summary>
        /// Image component for the equipment icon
        /// </summary>
        [SerializeField] private Image equipmentIcon;
        
        /// <summary>
        /// Whether this item is currently in an inventory
        /// </summary>
        [SerializeField] private bool isInInventory;
        
        /// <summary>
        /// Grid layout group for positioning drag cells
        /// </summary>
        [SerializeField] private GridLayoutGroup layoutGroup;
        
        /// <summary>
        /// Prefab for drag cell visualization
        /// </summary>
        [SerializeField] private UIItemDragCell dragCellPrefab;
        
        /// <summary>
        /// Rect transform component
        /// </summary>
        private RectTransform myRect;
        
        /// <summary>
        /// Counter for delaying hover event updates
        /// </summary>
        private int delayFrameToUpdateHoverEvent;
        
        /// <summary>
        /// 2D array of drag cells
        /// </summary>
        private UIItemDragCell[,] cells;
        
        /// <summary>
        /// Whether drag cells have been initialized
        /// </summary>
        private bool isInitCell;
        
        /// <summary>
        /// Whether the user is currently holding this item
        /// </summary>
        private bool isHoldingItem;
        
        /// <summary>
        /// Original parent transform before drag
        /// </summary>
        private Transform originalParent;
        
        /// <summary>
        /// Original position before drag
        /// </summary>
        private Vector3 originalPosition;
        
        /// <summary>
        /// Initializes this component
        /// </summary>
        private void Awake()
        {
            myRect = GetComponent<RectTransform>();
        }
        
        /// <summary>
        /// Updates the item position during drag
        /// </summary>
        private void Update()
        {
            if (!isHoldingItem)
            {
                return;
            }
            
            transform.position = Mouse.current.position.ReadValue();
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 5 == 0)
            {
                EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ UIItem = this});
            }
        }
        
        /// <summary>
        /// Initializes this item with equipment data
        /// </summary>
        /// <param name="equipment">The equipment data to represent</param>
        public void Init(EquipmentData equipment)
        {
            Item = new InventoryItem(equipment);
            equipmentIcon.sprite = equipment.IconSpr;
                
            CheckInitEmptyCell();
            SetItemDragData(equipment);
        }
        
        /// <summary>
        /// Initializes the drag cells if not already done
        /// </summary>
        private void CheckInitEmptyCell()
        {
            if (isInitCell)
            {
                return;
            } 
            
            layoutGroup.spacing = new Vector2(InventoryConstants.CELL_SPACING, InventoryConstants.CELL_SPACING);
            cells = new UIItemDragCell[InventoryConstants.MAX_EQUIPMENT_WIDTH, InventoryConstants.MAX_EQUIPMENT_HEIGHT];
            for (var i = 0; i < InventoryConstants.MAX_EQUIPMENT_WIDTH; i++)
            {
                for (var j = 0; j < InventoryConstants.MAX_EQUIPMENT_HEIGHT; j++)
                {
                    var cell = Instantiate(dragCellPrefab, layoutGroup.transform);
                    cell.SetVisible(false);
                    
                    cells[i, j] = cell;
                }
            }

            isInitCell = true;
        }
        
        /// <summary>
        /// Sets up the drag cells based on the equipment data
        /// </summary>
        /// <param name="equipment">The equipment data to represent</param>
        private void SetItemDragData(EquipmentData equipment)
        {
            var scaleSize = new Vector2(
                InventoryConstants.CELL_SIZE * equipment.Width + (InventoryConstants.CELL_SPACING * (equipment.Width - 1)), 
                InventoryConstants.CELL_SIZE * equipment.Height + (InventoryConstants.CELL_SPACING * (equipment.Height - 1)));
            
            myRect.sizeDelta = scaleSize;
            
            layoutGroup.cellSize = new Vector2(InventoryConstants.CELL_SIZE, InventoryConstants.CELL_SIZE);

            ToggleCells(equipment, true);
        }
        
        /// <summary>
        /// Toggles the visibility of drag cells based on the equipment dimensions
        /// </summary>
        /// <param name="equipment">The equipment data</param>
        /// <param name="isEnable">Whether to enable or disable cells</param>
        private void ToggleCells(EquipmentData equipment, bool isEnable)
        {
            for (var i = 0; i < InventoryConstants.MAX_EQUIPMENT_WIDTH; i++)
            {
                for (var j = 0; j < InventoryConstants.MAX_EQUIPMENT_HEIGHT; j++)
                {
                    cells[i, j].SetVisible(i < equipment.Width && j < equipment.Height && isEnable);
                }
            }
        }
        
        /// <summary>
        /// Marks whether this item is in an inventory
        /// </summary>
        /// <param name="isIn">Whether the item is in an inventory</param>
        public void MarkItemInInventory(bool isIn)
        {
            isInInventory = isIn;
        }
        
        /// <summary>
        /// Activates dragging functionality
        /// </summary>
        private void ActiveDragging()
        {
            transform.position = Mouse.current.position.ReadValue();
            
            EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ UIItem = this});
            
            isHoldingItem = true;
            delayFrameToUpdateHoverEvent = 0;
        }
        
        /// <summary>
        /// Called when the pointer is pressed down on this item
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInInventory)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnPickingEquipmentFromInventory()
                {
                    UIItemPick = this,
                });
                
                MarkItemInInventory(false);
            }

            ActiveDragging();
        }
        
        /// <summary>
        /// Called when the pointer is released from this item
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            isHoldingItem = false;
            
            EventManager.Instance.TriggerEvent(new EventData.OnPlacingEquipment()
            {
                UIItem = this,
                OnPlaceEquipmentInInventorySuccess = OnPlaceEquipmentSuccessInInventory
            });

            void OnPlaceEquipmentSuccessInInventory(EquipmentID id)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnRemoveEquipmentFromPlayerStorage()
                {
                    EquipmentID = Item.Equipment.EquipmentID
                });
            }
        }
        
        /// <summary>
        /// Called when dragging begins
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            delayFrameToUpdateHoverEvent = 0;
            transform.position = Mouse.current.position.ReadValue();

            // Store original values to restore if needed
            originalParent = transform.parent;
            originalPosition = transform.position;

            // Move to the top of the hierarchy for dragging
            transform.SetParent(transform.root);
            
            // Disable drag cells during drag for cleaner visuals
            ToggleCells(Item.Equipment, false);
        }
        
        /// <summary>
        /// Called during dragging
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Mouse.current.position.ReadValue();
            delayFrameToUpdateHoverEvent++;
            if (delayFrameToUpdateHoverEvent % 5 == 0)
            {
                EventManager.Instance.TriggerEvent(new EventData.DraggingEquipment(){ UIItem = this});
            }
        }
        
        /// <summary>
        /// Called when dragging ends
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            // Re-enable drag cells
            ToggleCells(Item.Equipment, true);
            
            // Check if we're over a valid inventory
            bool placedInInventory = false;
    
            // Get the current results from the event system
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
    
            for (int i = 0; i < results.Count; i++)
            {
                // Check if we hit an inventory cell
                UIInventoryCell inventoryCell = results[i].gameObject.GetComponent<UIInventoryCell>();
                if (inventoryCell != null && !inventoryCell.IsLocked && !inventoryCell.IsClaimed)
                {
                    // We found a valid cell in the minion inventory
                    placedInInventory = true;
                    break;
                }
            }
            
            // If not placed in inventory, return to original position
            if (!placedInInventory && !isInInventory)
            {
                transform.SetParent(originalParent);
                transform.position = originalPosition;
            }
        }
        
        /// <summary>
        /// Sets the position of this item in the grid
        /// </summary>
        /// <param name="gridPosition">The grid position</param>
        /// <param name="grid">The UI inventory grid</param>
        public void UpdatePosition(Vector2Int gridPosition, UIInventoryGrid grid)
        {
            if (grid == null)
                return;
                
            transform.position = grid.GetCenterPositionOfCellArea(
                gridPosition.x, gridPosition.y, Item.Width, Item.Height);
        }
    }
}