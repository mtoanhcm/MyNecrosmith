using GameUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Represents a cell in the inventory grid UI.
    /// Handles visual representation and interaction events for a single grid cell.
    /// </summary>
    public class UIInventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        /// <summary>
        /// Background image of the cell
        /// </summary>
        [SerializeField] private Image bg;
        
        /// <summary>
        /// X-position of this cell in the grid
        /// </summary>
        public int PosX { get; private set; }
        
        /// <summary>
        /// Y-position of this cell in the grid
        /// </summary>
        public int PosY { get; private set; }
        
        /// <summary>
        /// ID of the item claiming this cell
        /// </summary>
        public string ItemClaimID { get; private set; }
        
        /// <summary>
        /// Whether this cell is locked and unavailable
        /// </summary>
        public bool IsLocked { get; private set; }
        
        /// <summary>
        /// Whether this cell is claimed by an item
        /// </summary>
        public bool IsClaimed => !ItemClaimID.IsNulOrEmpty();
        
        /// <summary>
        /// Color to use when the cell is valid for item placement
        /// </summary>
        private Color validColor = Color.green;
        
        /// <summary>
        /// Color to use when the cell is invalid for item placement
        /// </summary>
        private Color invalidColor = Color.red;
        
        /// <summary>
        /// Color to use when the cell is locked
        /// </summary>
        private Color lockedColor = Color.gray;
        
        /// <summary>
        /// Color to use when the cell is normal (empty)
        /// </summary>
        private Color normalColor = Color.white;
        
        /// <summary>
        /// Event raised when the cell is clicked
        /// </summary>
        public System.Action<UIInventoryCell> OnCellClicked;
        
        /// <summary>
        /// Event raised when the mouse enters the cell
        /// </summary>
        public System.Action<UIInventoryCell> OnCellEnter;
        
        /// <summary>
        /// Event raised when the mouse exits the cell
        /// </summary>
        public System.Action<UIInventoryCell> OnCellExit;
        
        /// <summary>
        /// Initializes the cell with its grid position
        /// </summary>
        /// <param name="posX">X-position in the grid</param>
        /// <param name="posY">Y-position in the grid</param>
        public void Init(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
            
            SetLockCell(true);
        }
        
        /// <summary>
        /// Sets the item claiming this cell
        /// </summary>
        /// <param name="itemClaimID">ID of the claiming item, or empty to clear</param>
        public void SetItemClaim(string itemClaimID)
        {
            ItemClaimID = itemClaimID;
        }
        
        /// <summary>
        /// Sets whether this cell is locked
        /// </summary>
        /// <param name="isLocked">Whether to lock the cell</param>
        public void SetLockCell(bool isLocked)
        {
            IsLocked = isLocked;
            
            bg.raycastTarget = !isLocked;
            bg.color = isLocked ? lockedColor : normalColor;
        }
        
        /// <summary>
        /// Highlights the cell with the specified color
        /// </summary>
        /// <param name="color">Color to use for highlighting</param>
        public void Highlight(Color color)
        {
            if (!IsLocked)
            {
                bg.color = color;
            }
        }
        
        /// <summary>
        /// Resets the cell's highlight to its normal state
        /// </summary>
        public void ResetHighlight()
        {
            bg.color = IsLocked ? lockedColor : normalColor;
        }
        
        /// <summary>
        /// Called when the mouse enters the cell
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsLocked)
            {
                return;
            }
            
            bg.color = IsClaimed ? invalidColor : validColor;
            OnCellEnter?.Invoke(this);
        }
        
        /// <summary>
        /// Called when the mouse exits the cell
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsLocked)
            {
                return;
            }
            
            bg.color = normalColor;
            OnCellExit?.Invoke(this);
        }
        
        /// <summary>
        /// Called when the cell is clicked
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsLocked)
            {
                return;
            }
            
            OnCellClicked?.Invoke(this);
        }
        
        /// <summary>
        /// Called when the mouse hovers over the cell
        /// </summary>
        public void OnHoverOnCell()
        {
            if (IsLocked)
            {
                return;
            }

            bg.color = IsClaimed ? invalidColor : validColor;
            OnCellEnter?.Invoke(this);
        }

        /// <summary>
        /// Called when the mouse exits the cell
        /// </summary>
        public void OnExitHoverOnCell()
        {
            if (IsLocked)
            {
                return;
            }
    
            bg.color = normalColor;
            OnCellExit?.Invoke(this);
        }
    }
}