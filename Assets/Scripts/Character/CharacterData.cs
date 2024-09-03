using System;

namespace Character {
    public enum CharacterID { 
        SimpleMinion,
        SimpleEnemy
    }

    [Serializable]
    public struct StatData
    {
        public float HP;
        public float Speed;
        public float Damage;
        public float AttackSpeed;
        public float AttackRange;
        public float ScanRange;
    }
}
