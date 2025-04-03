using System.Collections.Generic;
using Config;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory.UI
{
    public class UIPlayerInventoryGrid : MonoBehaviour
    {
        [SerializeField] private UIPlayerInventoryCell cellPrefab;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private int rows = InventoryConstants.MAX_ROW;
        [SerializeField] private int columns = InventoryConstants.MAX_COLUMN;
        
        // Pool of cells for reuse
        private List<UIPlayerInventoryCell> cells = new List<UIPlayerInventoryCell>();
        
        // Currently selected cell index
        private int selectedCellIndex = -1;
        
        // Current category and page
        private EquipmentCategoryID currentCategory;
        private int currentPage;
        
        // Event delegate for cell selection
        public delegate void CellSelectedHandler(UIPlayerInventoryCell cell);
        public event CellSelectedHandler OnCellSelected;
        
        public void Init()
        {
            if (gridLayout == null)
                gridLayout = GetComponent<GridLayoutGroup>();
                
            // Set up the grid layout
            SetupGridLayout();
            
            // Create cells
            CreateCells();
        }
        
        /// <summary>
        /// Sets up the grid layout properties
        /// </summary>
        private void SetupGridLayout()
        {
            if (gridLayout == null)
                return;
                
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = columns;
            gridLayout.cellSize = new Vector2(InventoryConstants.CELL_SIZE, InventoryConstants.CELL_SIZE);
            gridLayout.spacing = new Vector2(InventoryConstants.CELL_SPACING, InventoryConstants.CELL_SPACING);
        }
        
        /// <summary>
        /// Creates all the cells for the grid
        /// </summary>
        private void CreateCells()
        {
            // Clear existing cells
            foreach (var cell in cells)
            {
                if (cell != null)
                    Destroy(cell.gameObject);
            }
            
            cells.Clear();
            
            // Create new cells
            int totalCells = rows * columns;
            for (int i = 0; i < totalCells; i++)
            {
                UIPlayerInventoryCell cell = Instantiate(cellPrefab, gridLayout.transform);
                cell.name = $"Cell_{i}";
                cell.Clear();
                cells.Add(cell);
            }
            
            Debug.Log("Create cell complete");
        }
        
        /// <summary>
        /// Updates the grid with inventory data
        /// </summary>
        /// <param name="category">The equipment category to display</param>
        /// <param name="page">The page to display</param>
        public void UpdateGrid(EquipmentCategoryID category, int page)
        {
            currentCategory = category;
            currentPage = page;
            
            // Get equipment slots for this page
            var slots = PlayerInventoryManager.Instance.GetEquipmentSlots(category, page);
            
            // Clear all cells first
            foreach (var cell in cells)
            {
                cell.Clear();
                cell.SetSelected(false);
            }
            
            // Set up cells with data
            for (int i = 0; i < slots.Count && i < cells.Count; i++)
            {
                cells[i].Setup(slots[i], i);
            }
            
            // Reset selection
            selectedCellIndex = -1;
            
            Debug.Log("Update grid");
        }
        
        /// <summary>
        /// Selects a cell by index
        /// </summary>
        /// <param name="index">The index of the cell to select</param>
        public void SelectCell(int index)
        {
            // Deselect previous cell
            if (selectedCellIndex >= 0 && selectedCellIndex < cells.Count)
            {
                cells[selectedCellIndex].SetSelected(false);
            }
            
            // Select new cell
            selectedCellIndex = index;
            
            if (selectedCellIndex >= 0 && selectedCellIndex < cells.Count)
            {
                cells[selectedCellIndex].SetSelected(true);
                OnCellSelected?.Invoke(cells[selectedCellIndex]);
            }
        }
        
        /// <summary>
        /// Gets the number of cells in the grid
        /// </summary>
        public int CellCount => cells.Count;
        
        /// <summary>
        /// Gets the current category being displayed
        /// </summary>
        public EquipmentCategoryID CurrentCategory => currentCategory;
        
        /// <summary>
        /// Gets the current page being displayed
        /// </summary>
        public int CurrentPage => currentPage;
    }
}