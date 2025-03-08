using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Manages the grid of cells in the inventory UI.
    /// Handles creating, positioning, and updating the cells.
    /// </summary>
    public class UIInventoryGrid : MonoBehaviour
    {
        /// <summary>
        /// Prefab for inventory cell
        /// </summary>
        [SerializeField] private UIInventoryCell cellPrefab;
        
        /// <summary>
        /// Grid layout group for positioning cells
        /// </summary>
        [SerializeField] private GridLayoutGroup gridLayout;
        
        /// <summary>
        /// RectTransform of the grid container
        /// </summary>
        private RectTransform rectTransform;
        
        /// <summary>
        /// 2D array of cell UI elements
        /// </summary>
        private UIInventoryCell[,] cells;
        
        /// <summary>
        /// Grid manager for inventory logic
        /// </summary>
        private InventoryGridManager gridManager;
        
        /// <summary>
        /// Number of rows in the grid
        /// </summary>
        private int rows;
        
        /// <summary>
        /// Number of columns in the grid
        /// </summary>
        private int columns;
        
        /// <summary>
        /// Event raised when a cell is clicked
        /// </summary>
        public System.Action<Vector2Int> OnCellClicked;
        
        /// <summary>
        /// Event raised when the mouse enters a cell
        /// </summary>
        public System.Action<Vector2Int> OnCellEnter;
        
        /// <summary>
        /// Event raised when the mouse exits a cell
        /// </summary>
        public System.Action<Vector2Int> OnCellExit;
        
        /// <summary>
        /// Initializes the grid with the specified dimensions
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <param name="gridManager">Grid manager for inventory logic</param>
        public void Initialize(int rows, int columns, InventoryGridManager gridManager)
        {
            this.rows = Mathf.Clamp(rows, InventoryConstants.MIN_ROW, InventoryConstants.MAX_ROW);
            this.columns = Mathf.Clamp(columns, InventoryConstants.MIN_COLUMN, InventoryConstants.MAX_COLUMN);
            this.gridManager = gridManager;
            
            rectTransform = GetComponent<RectTransform>();
            
            // Configure the grid layout
            ConfigureGridLayout();
            
            // Create cells
            CreateCells();
        }
        
        /// <summary>
        /// Configures the grid layout based on the dimensions
        /// </summary>
        private void ConfigureGridLayout()
        {
            if (gridLayout == null)
                gridLayout = GetComponent<GridLayoutGroup>();
                
            if (gridLayout == null)
                return;
                
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = columns;
            gridLayout.cellSize = new Vector2(InventoryConstants.CELL_SIZE, InventoryConstants.CELL_SIZE);
            gridLayout.spacing = new Vector2(InventoryConstants.CELL_SPACING, InventoryConstants.CELL_SPACING);
            
            // Set the size of the grid container
            Vector2 size = new Vector2(
                columns * (InventoryConstants.CELL_SIZE + InventoryConstants.CELL_SPACING) - InventoryConstants.CELL_SPACING,
                rows * (InventoryConstants.CELL_SIZE + InventoryConstants.CELL_SPACING) - InventoryConstants.CELL_SPACING
            );
            
            rectTransform.sizeDelta = size;
        }
        
        /// <summary>
        /// Creates the cell UI elements
        /// </summary>
        private void CreateCells()
        {
            // Clear any existing cells
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            cells = new UIInventoryCell[rows, columns];
            
            // Create cells
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    UIInventoryCell cell = Instantiate(cellPrefab, transform);
                    cell.name = $"Cell_{r}_{c}";
                    cell.Init(r, c);
                    
                    // Register event handlers
                    cell.OnCellClicked += OnCellClickedHandler;
                    cell.OnCellEnter += OnCellEnterHandler;
                    cell.OnCellExit += OnCellExitHandler;
                    
                    cells[r, c] = cell;
                }
            }
        }
        
        /// <summary>
        /// Handler for cell clicked events
        /// </summary>
        /// <param name="cell">The cell that was clicked</param>
        private void OnCellClickedHandler(UIInventoryCell cell)
        {
            OnCellClicked?.Invoke(new Vector2Int(cell.PosX, cell.PosY));
        }
        
        /// <summary>
        /// Handler for cell mouse enter events
        /// </summary>
        /// <param name="cell">The cell that was entered</param>
        private void OnCellEnterHandler(UIInventoryCell cell)
        {
            OnCellEnter?.Invoke(new Vector2Int(cell.PosX, cell.PosY));
        }
        
        /// <summary>
        /// Handler for cell mouse exit events
        /// </summary>
        /// <param name="cell">The cell that was exited</param>
        private void OnCellExitHandler(UIInventoryCell cell)
        {
            OnCellExit?.Invoke(new Vector2Int(cell.PosX, cell.PosY));
        }
        
        /// <summary>
        /// Updates the cell states based on the grid manager
        /// </summary>
        public void UpdateCellStates()
        {
            if (gridManager == null)
                return;
                
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GridCell gridCell = gridManager.GetCell(r, c);
                    UIInventoryCell uiCell = cells[r, c];
                    
                    if (gridCell != null && uiCell != null)
                    {
                        uiCell.SetLockCell(gridCell.State == GridCell.CellState.Locked);
                        uiCell.SetItemClaim(gridCell.ItemClaimID);
                    }
                }
            }
        }
        
        /// <summary>
        /// Highlights the specified cells
        /// </summary>
        /// <param name="positions">Set of positions to highlight</param>
        /// <param name="isValid">Whether the highlight is for a valid placement</param>
        public void HighlightCells(HashSet<(int, int)> positions, bool isValid)
        {
            Color highlightColor = isValid ? Color.green : Color.red;
            
            foreach (var pos in positions)
            {
                int r = pos.Item1;
                int c = pos.Item2;
                
                if (r >= 0 && r < rows && c >= 0 && c < columns)
                {
                    cells[r, c].Highlight(highlightColor);
                }
            }
        }
        
        /// <summary>
        /// Resets the highlight of all cells
        /// </summary>
        public void ResetHighlights()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    cells[r, c].ResetHighlight();
                }
            }
        }
        
        /// <summary>
        /// Gets the cell position from a world point
        /// </summary>
        /// <param name="worldPosition">The world position to check</param>
        /// <returns>Grid position if found, null otherwise</returns>
        public Vector2Int? GetCellPositionFromWorldPoint(Vector3 worldPosition)
        {
            // Convert world position to local position in the grid
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, worldPosition, null, out Vector2 localPosition))
            {
                // Calculate the cell position based on local position
                float cellWidth = InventoryConstants.CELL_SIZE + InventoryConstants.CELL_SPACING;
                float cellHeight = InventoryConstants.CELL_SIZE + InventoryConstants.CELL_SPACING;
                
                int col = Mathf.FloorToInt(localPosition.x / cellWidth);
                int row = Mathf.FloorToInt(localPosition.y / cellHeight);
                
                // Check if the position is within bounds
                if (row >= 0 && row < rows && col >= 0 && col < columns)
                {
                    return new Vector2Int(row, col);
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Gets the world position of a cell
        /// </summary>
        /// <param name="gridPosition">The grid position</param>
        /// <returns>World position of the cell</returns>
        public Vector3 GetCellWorldPosition(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0 || gridPosition.x >= rows || 
                gridPosition.y < 0 || gridPosition.y >= columns)
                return Vector3.zero;
                
            return cells[gridPosition.x, gridPosition.y].transform.position;
        }
        
        /// <summary>
        /// Gets the world position at the center of a rectangular area
        /// </summary>
        /// <param name="startRow">Starting row</param>
        /// <param name="startColumn">Starting column</param>
        /// <param name="width">Width in cells</param>
        /// <param name="height">Height in cells</param>
        /// <returns>World position at the center of the area</returns>
        public Vector3 GetCenterPositionOfCellArea(int startRow, int startColumn, int width, int height)
        {
            // Check if positions are within bounds
            if (startRow < 0 || startRow + height > rows || 
                startColumn < 0 || startColumn + width > columns)
                return Vector3.zero;
                
            // Get the positions of the top-left and bottom-right cells
            Vector3 topLeftPos = cells[startRow, startColumn].transform.position;
            Vector3 bottomRightPos = cells[startRow + height - 1, startColumn + width - 1].transform.position;
            
            // Return the center position
            return (topLeftPos + bottomRightPos) * 0.5f;
        }
    }
}