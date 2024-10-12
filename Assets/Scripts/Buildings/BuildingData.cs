using System;
using UnityEngine;

namespace Building {

    [Serializable]
    public struct BuildingData
    {
        public BuildingType Type;
        public float HP;
        public int Level;
        public float TimeToStartActivation;
        public float TimeToDelayActivation;
    }

    public enum BuildingType { 
        None,
        Treasure,
        EnemyBase,
        MainBase
    }
}
