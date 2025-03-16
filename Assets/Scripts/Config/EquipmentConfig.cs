using Minion.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    public abstract class EquipmentConfig : ScriptableObject
    {
        public EquipmentID EquipmentID;
        public EquipmentCategoryID CategoryID;
        public Rarity Rarity;
        public string EquipmentName;
        public int LoadPoint;
        public Sprite Icon; // Sprite representing the weapon
        
        [MinValue(1) ,MaxValue(MinionInventoryParam.MAX_EQUIPMENT_WIDTH)]
        public int Width;
        [MinValue(1) ,MaxValue(MinionInventoryParam.MAX_EQUIPMENT_HEIGHT)]
        public int Height;
    }
}
