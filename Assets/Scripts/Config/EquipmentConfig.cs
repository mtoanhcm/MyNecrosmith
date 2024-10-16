using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Config
{
    [CreateAssetMenu(fileName = "EquipmentConfig", menuName = "Config/EquipmentConfig")]
    public class EquipmentConfig : ScriptableObject
    {
        [SerializeField]
        private List<EquipmentStruct> equipments; //List of weapons
    }
    
    [System.Serializable]
    public struct EquipmentStruct
    {
        public ItemID ID; // ID of the weapon
        public string EquipmentName;
        public int EffectValue;
        public Sprite icon; // Sprite representing the weapon
        public List<Vector2Int> Shapes; // List of coordinates representing the shape of the weapon

        public void RotateShape()
        {
            for (var i = 0; i < Shapes.Count; i++)
            {
                Shapes[i] = new Vector2Int(-Shapes[i].y, Shapes[i].x); // Rotate 90 degrees
            }
        }
    }
}
