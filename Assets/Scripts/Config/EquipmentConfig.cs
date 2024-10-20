using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EquipmentConfig", menuName = "Config/EquipmentConfig")]
    public class EquipmentConfig : ScriptableObject
    {
        public ItemID ID; // ID of the weapon
        public string EquipmentName;
        public int EffectValue;
        public Sprite Icon; // Sprite representing the weapon
        public int Width;
        public int Height;
    }
}
