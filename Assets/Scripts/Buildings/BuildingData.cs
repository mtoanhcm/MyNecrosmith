using System;

namespace Building {

    [Serializable]
    public struct BuildingData
    {
        public BuildingType Type;
        public int HP;
        public int Level;
    }

    public enum BuildingType { 
        Treasure,
        EnemyBase
    }
}
