using Inventory.UI;
using Observer;
using Player.Inventory.UI;
using UnityEngine;

namespace Player.Inventory
{
    /// <summary>
    /// Coordinates between the player inventory and minion inventory systems.
    /// Handles transferring items between the two inventories.
    /// </summary>
    public class InventoryCoordinator : MonoBehaviour
    {
        [SerializeField] private UIPlayerInventoryPanel playerInventoryPanel;
        [SerializeField] private UIInventoryView minionInventoryView;
        
        [Header("UI Toggle")]
        [SerializeField] private KeyCode togglePlayerInventoryKey = KeyCode.Tab;
        [SerializeField] private KeyCode toggleMinionInventoryKey = KeyCode.I;
        
        // Whether the player inventory is currently visible
        private bool isPlayerInventoryVisible = false;
        
        // Whether the minion inventory is currently visible
        private bool isMinionInventoryVisible = false;
        
        private void Start()
        {
            // Subscribe to events
            EventManager.Instance.StartListening<EventData.OnPlacingEquipment>(OnPlacingEquipment);
            EventManager.Instance.StartListening<EventData.OpenCharacterInventory>(OnOpenCharacterInventory);
            
            // Hide both inventories initially
            if (playerInventoryPanel != null)
                playerInventoryPanel.gameObject.SetActive(false);
            
            if (minionInventoryView != null)
                minionInventoryView.gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            EventManager.Instance?.StopListening<EventData.OnPlacingEquipment>(OnPlacingEquipment);
            EventManager.Instance?.StopListening<EventData.OpenCharacterInventory>(OnOpenCharacterInventory);
        }
        
        private void Update()
        {
            // Toggle player inventory
            if (Input.GetKeyDown(togglePlayerInventoryKey))
            {
                TogglePlayerInventory();
            }
            
            // Toggle minion inventory
            if (Input.GetKeyDown(toggleMinionInventoryKey))
            {
                ToggleMinionInventory();
            }
        }
        
        /// <summary>
        /// Toggles the player inventory visibility
        /// </summary>
        public void TogglePlayerInventory()
        {
            isPlayerInventoryVisible = !isPlayerInventoryVisible;
            
            if (playerInventoryPanel != null)
                playerInventoryPanel.gameObject.SetActive(isPlayerInventoryVisible);
        }
        
        /// <summary>
        /// Toggles the minion inventory visibility
        /// </summary>
        public void ToggleMinionInventory()
        {
            // If there's no open minion inventory, show a message
            if (!isMinionInventoryVisible && minionInventoryView.GetCurrentInventory() == null)
            {
                Debug.Log("No minion inventory is currently open. Select a minion first.");
                return;
            }
            
            isMinionInventoryVisible = !isMinionInventoryVisible;
            
            if (minionInventoryView != null)
                minionInventoryView.gameObject.SetActive(isMinionInventoryVisible);
        }
        
        /// <summary>
        /// Shows both player and minion inventories side by side
        /// </summary>
        public void ShowBothInventories()
        {
            // If there's no open minion inventory, show a message
            if (minionInventoryView.GetCurrentInventory() == null)
            {
                Debug.Log("No minion inventory is currently open. Select a minion first.");
                return;
            }
            
            isPlayerInventoryVisible = true;
            isMinionInventoryVisible = true;
            
            if (playerInventoryPanel != null)
                playerInventoryPanel.gameObject.SetActive(true);
                
            if (minionInventoryView != null)
                minionInventoryView.gameObject.SetActive(true);
                
            // Position them side by side
            PositionInventoriesSideBySide();
        }
        
        /// <summary>
        /// Positions the player and minion inventories side by side
        /// </summary>
        private void PositionInventoriesSideBySide()
        {
            if (playerInventoryPanel == null || minionInventoryView == null)
                return;
                
            // Get the screen dimensions
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            // Position player inventory on the left
            RectTransform playerRT = playerInventoryPanel.GetComponent<RectTransform>();
            if (playerRT != null)
            {
                playerRT.anchoredPosition = new Vector2(-screenWidth * 0.25f, 0);
            }
            
            // Position minion inventory on the right
            RectTransform minionRT = minionInventoryView.GetComponent<RectTransform>();
            if (minionRT != null)
            {
                minionRT.anchoredPosition = new Vector2(screenWidth * 0.25f, 0);
            }
        }
        
        /// <summary>
        /// Handles equipment being placed
        /// </summary>
        /// <param name="data">Event data for equipment placement</param>
        private void OnPlacingEquipment(EventData.OnPlacingEquipment data)
        {
            // If the equipment is being placed in the minion inventory
            if (minionInventoryView != null && minionInventoryView.gameObject.activeSelf)
            {
                // Remove the equipment from player inventory when successfully added to minion
                data.OnPlaceEquipmentInInventorySuccess = (equipmentID) =>
                {
                    // Find and remove the equipment from player inventory
                    PlayerInventoryManager.Instance.RemoveEquipment(data.UIItem.Item.Equipment);
                };
            }
        }
        
        /// <summary>
        /// Handles opening a minion inventory
        /// </summary>
        /// <param name="data">Event data for opening character inventory</param>
        private void OnOpenCharacterInventory(EventData.OpenCharacterInventory data)
        {
            // Update minion inventory visibility flag
            isMinionInventoryVisible = data.InventoryData != null;
            
            // If opening a minion inventory, show player inventory too
            if (data.InventoryData != null)
            {
                ShowBothInventories();
            }
        }
    }
}