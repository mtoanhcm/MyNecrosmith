using Character;
using Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIInventoryItem : MonoBehaviour
    {
        public RectTransform MyRect => myRect;
        public InventoryItem Item { get; private set; }

        [SerializeField] private Image equipmentIconImg;
        
        private RectTransform myRect;
        
        private void Awake()
        {
            myRect = GetComponent<RectTransform>();
        }

        public void Init(EquipmentData equipment)
        {
            Item = new InventoryItem(equipment);

            equipmentIconImg.sprite = equipment.IconSpr;
            
            ResizeItemWithEquipment(equipment);
        }

        private void ResizeItemWithEquipment(EquipmentData equipment)
        {
            var scaleSize = new Vector2(
                InventoryParam.CELL_SIZE * equipment.Width + (InventoryParam.CELL_SPACING * (equipment.Width - 1)), 
                InventoryParam.CELL_SIZE * equipment.Height + (InventoryParam.CELL_SPACING * (equipment.Height - 1)));
            
            myRect.sizeDelta = scaleSize;
        }
    }   
}
