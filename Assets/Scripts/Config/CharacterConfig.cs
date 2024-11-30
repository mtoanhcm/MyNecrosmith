using Character;
using UnityEngine;

namespace  Config
{
    public class CharacterConfig : ScriptableObject
    {
        public int HP;
        public float MoveSpeed;
        public float AttackSpeed;
    }
    
    public enum CharacterClass
    {
        HumanKnight
    }

    public enum CharacterRace
    {
        Human
    }
}
