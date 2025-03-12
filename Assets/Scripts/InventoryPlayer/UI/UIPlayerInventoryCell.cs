using Equipment;
using Inventory.UI;
using Observer;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Inventory.UI
{
    public class UIPlayerInventoryCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image equipmentIconImage;
        [SerializeField] private TextMeshProUGUI stackCountText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image rarityBorderImage;
        
        // The equipment slot data this cell represents
        private PlayerInventoryData.EquipmentSlot equipmentSlot;
        
        // Whether this cell is currently selected
        private bool isSelected;
        
        // Cell index in the grid
        private int cellIndex;
        
        // Colors for different states
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.cyan;
        [SerializeField] private Color hoverColor = Color.yellow;
        
        /// <summary>
        /// Sets up the cell with equipment data
        /// </summary>
        /// <param name="slot">The equipment slot data</param>
        /// <param name="index">The cell index in the grid</param>
        public void Setup(PlayerInventoryData.EquipmentSlot slot, int index)
        {
            this.equipmentSlot = slot;
            this.cellIndex = index;
            
            // Update visual elements
            UpdateVisuals();
        }
        
        /// <summary>
        /// Updates the visual appearance of the cell
        /// </summary>
        private void UpdateVisuals()
        {
            bool hasEquipment = equipmentSlot != null && equipmentSlot.Equipment != null;
            
            // Set active states
            equipmentIconImage.gameObject.SetActive(hasEquipment);
            stackCountText.gameObject.SetActive(hasEquipment && equipmentSlot.StackCount > 1);
            levelText.gameObject.SetActive(hasEquipment);
            //rarityBorderImage.gameObject.SetActive(hasEquipment);
            
            if (hasEquipment)
            {
                // Set icon
                equipmentIconImage.sprite = equipmentSlot.Equipment.IconSpr;
                
                // Set stack count
                stackCountText.text = equipmentSlot.StackCount.ToString();
                
                // Set level
                //levelText.text = "Lv." + equipmentSlot.Equipment.GetLevel();
                
                // Set rarity border color
                //EquipmentRarity rarity = equipmentSlot.Equipment.GetRarity();
                //rarityBorderImage.color = EquipmentDataExtension.GetRarityColor(rarity);
            }
            
            // Set background color based on state
            backgroundImage.color = isSelected ? selectedColor : normalColor;
        }
        
        /// <summary>
        /// Gets the equipment data from this cell
        /// </summary>
        /// <returns>The equipment data, or null if cell is empty</returns>
        public EquipmentData GetEquipment()
        {
            return equipmentSlot?.Equipment;
        }
        
        /// <summary>
        /// Gets the equipment slot data from this cell
        /// </summary>
        /// <returns>The equipment slot data, or null if cell is empty</returns>
        public PlayerInventoryData.EquipmentSlot GetEquipmentSlot()
        {
            return equipmentSlot;
        }
        
        /// <summary>
        /// Sets whether this cell is selected
        /// </summary>
        /// <param name="selected">Whether to select the cell</param>
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            backgroundImage.color = isSelected ? selectedColor : normalColor;
        }
        
        /// <summary>
        /// Clears the cell data
        /// </summary>
        public void Clear()
        {
            equipmentSlot = null;
            isSelected = false;
            UpdateVisuals();
        }
        
        /// <summary>
        /// Handles cell click events
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (equipmentSlot != null && equipmentSlot.Equipment != null)
            {
                // Notify that this equipment was selected
                // EventManager.Instance.TriggerEvent(new EventData.OnChooseEquipmentInStorage
                // {
                //     Equipment = equipmentSlot.Equipment
                // });
                
                // Create a draggable item for this equipment
                if (equipmentSlot.StackCount > 0)
                {
                    CreateDraggableItem();
                }
            }
        }
        
        /// <summary>
        /// Creates a draggable item from this cell
        /// </summary>
        private async void CreateDraggableItem()
        {
            if (equipmentSlot == null || equipmentSlot.Equipment == null)
                return;

            // Create request data
            var requestData = new EventData.RequestUIInventoryItem
            {
                Equipment = equipmentSlot.Equipment,
                OnItemCreated = OnUIItemCreated
            };
    
            // Trigger the event
            EventManager.Instance.TriggerEvent(requestData);

            void OnUIItemCreated(UIInventoryItem item)
            {
                // Initialize it with our equipment data
                item.Init(equipmentSlot.Equipment);
    
                // Position it at the mouse
                item.transform.SetParent(transform.root);
                item.transform.position = Input.mousePosition;
            }
        }
        
        /// <summary>
        /// Handles pointer enter events
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (equipmentSlot != null && equipmentSlot.Equipment != null && !isSelected)
            {
                backgroundImage.color = hoverColor;
                
                // Show the tooltip
                EventManager.Instance.TriggerEvent(new EventData.OnChooseEquipmentInStorage
                {
                    Equipment = equipmentSlot.Equipment
                });
            }
            else
            {
                EventManager.Instance.TriggerEvent(new EventData.OnChooseEquipmentInStorage
                {
                    Equipment = null
                });
            }
        }
        
        /// <summary>
        /// Handles pointer exit events
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isSelected)
            {
                backgroundImage.color = normalColor;
            }
        }
    }
}