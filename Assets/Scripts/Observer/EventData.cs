using Character;
using Equipment;
using UI;
using UnityEngine;

namespace Observer {
    public class EventData
    {
        /// <summary>
        /// Open fog of war by radius
        /// </summary>
        public struct OpenCharacterInventory
        {
            public Inventory InventoryData;
        }

        public struct DraggingEquipment
        {
            public UIItemDragAndDrop DragItem;
        }

        public struct OnPlacingEquipment
        {
            public EquipmentData Equipment;
            public RectTransform EquipementRect;
        }
    }
}
