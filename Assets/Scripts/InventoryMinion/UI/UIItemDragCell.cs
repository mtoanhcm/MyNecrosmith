using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Represents a visual cell in a draggable inventory item.
    /// Used to visualize the grid cells an item occupies.
    /// </summary>
    public class UIItemDragCell : MonoBehaviour
    {
        /// <summary>
        /// The RectTransform of this cell
        /// </summary>
        public RectTransform RectTrans => rectTrans;
        
        /// <summary>
        /// Whether this cell is currently visible
        /// </summary>
        public bool IsVisible => gameObject.activeSelf;
        
        /// <summary>
        /// The RectTransform component
        /// </summary>
        private RectTransform rectTrans;
        
        /// <summary>
        /// The Image component for visual display
        /// </summary>
        [SerializeField] private Image cellImage;
        
        /// <summary>
        /// Color when the cell is over a valid position
        /// </summary>
        [SerializeField] private Color validColor = new Color(0.2f, 0.8f, 0.2f, 0.5f);
        
        /// <summary>
        /// Color when the cell is over an invalid position
        /// </summary>
        [SerializeField] private Color invalidColor = new Color(0.8f, 0.2f, 0.2f, 0.5f);
        
        /// <summary>
        /// Color when the cell is in normal state
        /// </summary>
        [SerializeField] private Color normalColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        
        /// <summary>
        /// Initializes this component
        /// </summary>
        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
            
            if (cellImage == null)
                cellImage = GetComponent<Image>();
                
            if (cellImage != null)
                cellImage.color = normalColor;
        }
        
        /// <summary>
        /// Sets whether this cell is visible
        /// </summary>
        /// <param name="isVisible">Whether to show or hide this cell</param>
        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
            
            if (isVisible)
                SetNormalState();
        }
        
        /// <summary>
        /// Sets this cell's visual state to indicate a valid position
        /// </summary>
        public void SetValidState()
        {
            if (cellImage != null)
                cellImage.color = validColor;
        }
        
        /// <summary>
        /// Sets this cell's visual state to indicate an invalid position
        /// </summary>
        public void SetInvalidState()
        {
            if (cellImage != null)
                cellImage.color = invalidColor;
        }
        
        /// <summary>
        /// Sets this cell's visual state to its normal appearance
        /// </summary>
        public void SetNormalState()
        {
            if (cellImage != null)
                cellImage.color = normalColor;
        }
    }
}