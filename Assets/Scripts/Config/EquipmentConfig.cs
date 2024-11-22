using System.Collections;
using System.Collections.Generic;
using Character;
using Equipment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    public abstract class EquipmentConfig : ScriptableObject
    {
        public string EquipmentName;
        public int LoadPoint;
        public Sprite Icon; // Sprite representing the weapon
        
        [MinValue(1) ,MaxValue(InventoryParam.MAX_EQUIPMENT_WIDTH)]
        public int Width;
        [MinValue(1) ,MaxValue(InventoryParam.MAX_EQUIPMENT_HEIGHT)]
        public int Height;
    }
}
