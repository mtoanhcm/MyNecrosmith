using Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config {

    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Config/BuildingConfig", order = 1)]
    public class BuildingConfig : ScriptableObject
    {
        public BuildingBase GoldMine => goldMine;

        [SerializeField]
        private BuildingBase goldMine;
    }
}
