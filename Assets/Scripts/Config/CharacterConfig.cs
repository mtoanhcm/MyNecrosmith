using Character;
using UnityEngine;

namespace  Config
{
    public class CharacterConfig : ScriptableObject
    {
        public CharacterID ID;
        public int HP;
        public int MoveSpeed;
        public int AttackSpeed;
        public int ViewRadius;
    }
    
    public enum CharacterID
    {
        HumanKnight = 0,
        
        BareHandZombie = 100,
        KnifeZombie = 101
    }

    public enum CharacterRace
    {
        Human
    }
}
