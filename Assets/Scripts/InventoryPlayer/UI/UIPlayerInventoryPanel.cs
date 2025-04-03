using System.Collections.Generic;
using Config;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory.UI
{
    public class UIPlayerInventoryPanel : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private UIPlayerInventoryGrid inventoryGrid;
        
        [Header("Navigation")]
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private TextMeshProUGUI pageInfoText;
        
        [Header("Category Tabs")]
        [SerializeField] private Transform tabContainer;
        [SerializeField] private Button tabButtonPrefab;
        [SerializeField] private Color activeTabColor = Color.cyan;
        [SerializeField] private Color inactiveTabColor = Color.grey;
        
        // The current category being displayed
        private EquipmentCategoryID currentCategory;
        
        // The current page being displayed
        private int currentPage;
        
        // The total number of pages for the current category
        private int totalPages;
        
        // Dictionary of category tab buttons
        private Dictionary<EquipmentCategoryID, Button> categoryTabs = new Dictionary<EquipmentCategoryID, Button>();
        
        // Dictionary of tab button original colors
        private Dictionary<Button, Color> tabOriginalColors = new Dictionary<Button, Color>();
        
        private void Awake()
        {
            // Set up navigation buttons
            nextPageButton.onClick.AddListener(NextPage);
            prevPageButton.onClick.AddListener(PrevPage);
            
            // Create category tabs
            CreateCategoryTabs();
            inventoryGrid.Init();
        }
        
        private void OnEnable()
        {
            // Subscribe to events
            PlayerInventoryManager.Instance.OnInventoryChanged += OnInventoryChanged;
            
            // Initialize with the first available category
            InitializeWithDefaultCategory();
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            PlayerInventoryManager.Instance.OnInventoryChanged -= OnInventoryChanged;
        }
        
        /// <summary>
        /// Creates all category tabs for the inventory
        /// </summary>
        private void CreateCategoryTabs()
        {
            // Clear existing tabs
            foreach (Transform child in tabContainer)
            {
                Destroy(child.gameObject);
            }
            
            categoryTabs.Clear();
            tabOriginalColors.Clear();
            
            // Create a tab for each equipment category
            foreach (EquipmentCategoryID category in System.Enum.GetValues(typeof(EquipmentCategoryID)))
            {
                if (category == EquipmentCategoryID.None)
                    continue;
                    
                // Create the tab button
                Button tabButton = Instantiate(tabButtonPrefab, tabContainer);
                tabButton.name = $"Tab_{category}";
                
                // Set the tab text
                TextMeshProUGUI tabText = tabButton.GetComponentInChildren<TextMeshProUGUI>();
                if (tabText != null)
                {
                    tabText.text = category.ToString();
                }
                
                // Set the tab image color
                Image tabImage = tabButton.GetComponent<Image>();
                if (tabImage != null)
                {
                    tabOriginalColors[tabButton] = tabImage.color;
                }
                
                // Set up the tab click event
                EquipmentCategoryID capturedCategory = category; // Capture for lambda
                tabButton.onClick.AddListener(() => SwitchCategory(capturedCategory));
                
                // Add to dictionary
                categoryTabs[category] = tabButton;
            }
        }
        
        /// <summary>
        /// Initializes the panel with the first available category
        /// </summary>
        private void InitializeWithDefaultCategory()
        {
            // Get all populated categories
            var populatedCategories = PlayerInventoryManager.Instance.GetPopulatedCategories();
            
            // Use the first populated category, or default to Weapon if none
            EquipmentCategoryID defaultCategory = populatedCategories.Count > 0 
                ? populatedCategories[0] 
                : EquipmentCategoryID.Sword;
                
            // Switch to the default category
            SwitchCategory(defaultCategory);
        }
        
        /// <summary>
        /// Switches to display a different equipment category
        /// </summary>
        /// <param name="category">The category to display</param>
        private void SwitchCategory(EquipmentCategoryID category)
        {
            currentCategory = category;
            currentPage = 0;
            
            // Update tab visuals
            UpdateTabVisuals();
            
            // Update the grid
            RefreshGrid();
        }
        
        /// <summary>
        /// Updates the visual appearance of tabs
        /// </summary>
        private void UpdateTabVisuals()
        {
            foreach (var kvp in categoryTabs)
            {
                EquipmentCategoryID category = kvp.Key;
                Button tabButton = kvp.Value;
                
                Image tabImage = tabButton.GetComponent<Image>();
                if (tabImage != null)
                {
                    // Set active/inactive color
                    tabImage.color = category == currentCategory 
                        ? activeTabColor 
                        : inactiveTabColor;
                        
                    // Add indicator for populated categories
                    bool hasItems = PlayerInventoryManager.Instance.HasItems(category);
                    tabButton.interactable = hasItems;
                }
            }
        }
        
        /// <summary>
        /// Goes to the next page of items
        /// </summary>
        public void NextPage()
        {
            if (currentPage < totalPages - 1)
            {
                currentPage++;
                RefreshGrid();
            }
        }
        
        /// <summary>
        /// Goes to the previous page of items
        /// </summary>
        public void PrevPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                RefreshGrid();
            }
        }
        
        /// <summary>
        /// Refreshes the item grid and navigation controls
        /// </summary>
        private void RefreshGrid()
        {
            // Update total pages
            totalPages = PlayerInventoryManager.Instance.GetPageCount(currentCategory);
            
            // Update page info text
            if (totalPages > 0)
            {
                pageInfoText.text = $"{currentPage + 1}/{totalPages}";
            }
            else
            {
                pageInfoText.text = "0";
            }
            
            // Update navigation buttons
            nextPageButton.interactable = currentPage < totalPages - 1;
            prevPageButton.interactable = currentPage > 0;
            
            // Update the grid with items
            inventoryGrid.UpdateGrid(currentCategory, currentPage);
        }
        
        /// <summary>
        /// Event handler for inventory changes
        /// </summary>
        /// <param name="category">The category that changed</param>
        private void OnInventoryChanged(EquipmentCategoryID category)
        {
            // Update tabs to show which categories have items
            UpdateTabVisuals();
            
            // Refresh the grid if the current category changed
            if (category == currentCategory)
            {
                RefreshGrid();
            }
        }
    }
}