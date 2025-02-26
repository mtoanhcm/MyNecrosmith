using Building;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Config {
    public class BuildingConfig : ScriptableObject
    {
        public BuildingID ID;
        public ArmorType ArmorType;
        public int HP;
    }
}
