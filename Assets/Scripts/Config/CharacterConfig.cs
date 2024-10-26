using Character;
using UnityEngine;

namespace  Config
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/Character" +
                                                              "Config")]
    public class CharacterConfig : ScriptableObject
    {
        public C_Race Race;
        public C_Class Class;
        public int HP;
        public float MoveSpeed;
        public float AttacSpeed;
        public Vector2 InventorySize;
    }
}
