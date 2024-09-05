using System;

namespace Building {

    [Serializable]
    public struct BuildingData
    {
        public BuildingType Type;
        public float HP;
        public int Level;
    }

    public enum BuildingType { 
        Treasure,
        EnemyBase,
        MainBase
    }
}
