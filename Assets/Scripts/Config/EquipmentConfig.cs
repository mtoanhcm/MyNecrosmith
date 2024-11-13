using System.Collections;
using System.Collections.Generic;
using Character;
using Equipment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EquipmentConfig", menuName = "Config/EquipmentConfig")]
    public class EquipmentConfig : ScriptableObject
    {
        public EquipmentID ID; // ID of the weapon
        public EquipmentCategoryID CategoryID;
        public string EquipmentName;
        public int EffectValue;
        public float AttackRadius;
        public float AttackSpeed;
        public Sprite Icon; // Sprite representing the weapon
        
        [MinValue(1) ,MaxValue(InventoryParam.MAX_EQUIPMENT_WIDTH)]
        public int Width;
        [MinValue(1) ,MaxValue(InventoryParam.MAX_EQUIPMENT_HEIGHT)]
        public int Height;
    }
}
