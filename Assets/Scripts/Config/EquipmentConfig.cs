using System.Collections;
using System.Collections.Generic;
using Equipment;
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
        public Sprite Icon; // Sprite representing the weapon
        public EquipmentBase EquipmentPrefab;
        public int Width;
        public int Height;
    }
}
