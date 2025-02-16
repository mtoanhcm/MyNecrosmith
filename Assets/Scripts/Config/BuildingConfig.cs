using Building;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config {
    public class BuildingConfig : ScriptableObject
    {
        public BuildingType BuildingType;
        public ArmorType ArmorType;
        public int HP;
    }
}
