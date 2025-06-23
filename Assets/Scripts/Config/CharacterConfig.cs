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
        None = 0,
        HumanKnight = 1,

        Skeleton = 100,
        KnifeZombie = 101,
            Orc = 102
    }

    public enum CharacterRace
    {
        Human
    }
}
