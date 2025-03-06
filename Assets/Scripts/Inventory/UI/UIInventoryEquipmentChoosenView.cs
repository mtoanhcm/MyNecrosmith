using Config;
using Equipment;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Displays detailed information about equipment items.
    /// Shows name, stats, and other properties of selected equipment.
    /// </summary>
    public class UIInventoryEquipmentChoosenView : MonoBehaviour
    {
        /// <summary>
        /// Text component for the equipment name
        /// </summary>
        [SerializeField] private TextMeshProUGUI equipmentNameTxt;
        
        /// <summary>
        /// Text component for the effect attribute
        /// </summary>
        [SerializeField] private TextMeshProUGUI effectAttributeTxt;
        
        /// <summary>
        /// Text component for the equipment type
        /// </summary>
        [SerializeField] private TextMeshProUGUI equipmentTypeTxt;
        
        /// <summary>
        /// Text component for the load point
        /// </summary>
        [SerializeField] private TextMeshProUGUI loadPointTxt;
        
        /// <summary>
        /// Prefab for the equipment UI item
        /// </summary>
        [SerializeField] private UIInventoryItem equipmentUIItemPrefab;
        
        /// <summary>
        /// Container for the equipment UI item
        /// </summary>
        [SerializeField] private Transform equipmentUIItemContainer;
        
        /// <summary>
        /// Currently displayed equipment UI item
        /// </summary>
        private UIInventoryItem equipmentUIItem;
        
        /// <summary>
        /// Currently displayed equipment data
        /// </summary>
        private EquipmentData currentEquipment;
        
        /// <summary>
        /// Called when the component is enabled
        /// </summary>
        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.OnChooseEquipmentInStorage>(OnChooseEquipmentInStorage);
        }
        
        /// <summary>
        /// Called when the component is disabled
        /// </summary>
        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OnChooseEquipmentInStorage>(OnChooseEquipmentInStorage);
        }
        
        /// <summary>
        /// Shows details for the specified equipment
        /// </summary>
        /// <param name="equipment">The equipment to display</param>
        public void ShowEquipmentDetails(EquipmentData equipment)
        {
            if (equipment == null)
            {
                ResetInfo();
                return;
            }
            
            currentEquipment = equipment;
            
            // Update text components
            equipmentNameTxt.text = equipment.Name;
            effectAttributeTxt.text = equipment.EffectValue;
            equipmentTypeTxt.text = equipment.EffectType;
            loadPointTxt.text = $"Load point: {equipment.LoadPoint}";
            
            // Update equipment UI item
            UpdateEquipmentUIItem(equipment);
        }
        
        /// <summary>
        /// Updates the equipment UI item
        /// </summary>
        /// <param name="equipment">The equipment to display</param>
        private void UpdateEquipmentUIItem(EquipmentData equipment)
        {
            // Clean up existing UI item
            if (equipmentUIItem != null)
            {
                Destroy(equipmentUIItem.gameObject);
            }
            
            // Create new UI item
            equipmentUIItem = Instantiate(equipmentUIItemPrefab, equipmentUIItemContainer);
            equipmentUIItem.Init(equipment);
            
            // Position the UI item in the center of the container
            equipmentUIItem.transform.localPosition = Vector3.zero;
            
            // Make it non-interactive
            Destroy(equipmentUIItem.GetComponent<UnityEngine.EventSystems.EventTrigger>());
        }
        
        /// <summary>
        /// Called when an equipment is chosen from storage
        /// </summary>
        /// <param name="data">Event data for the chosen equipment</param>
        private void OnChooseEquipmentInStorage(EventData.OnChooseEquipmentInStorage data)
        {
            if (data.Equipment == null)
            {
                ResetInfo();
                return;
            }

            ShowEquipmentDetails(data.Equipment);
        }
        
        /// <summary>
        /// Resets the displayed information
        /// </summary>
        private void ResetInfo()
        {
            equipmentNameTxt.text = string.Empty;
            effectAttributeTxt.text = string.Empty;
            equipmentTypeTxt.text = string.Empty;
            loadPointTxt.text = string.Empty;
            
            // Clean up existing UI item
            if (equipmentUIItem != null)
            {
                Destroy(equipmentUIItem.gameObject);
                equipmentUIItem = null;
            }
            
            currentEquipment = null;
        }
        
        /// <summary>
        /// Gets the currently displayed equipment
        /// </summary>
        /// <returns>The current equipment, or null if none is displayed</returns>
        public EquipmentData GetCurrentEquipment()
        {
            return currentEquipment;
        }
    }
}