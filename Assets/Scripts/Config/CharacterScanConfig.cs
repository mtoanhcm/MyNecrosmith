using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config {

    [CreateAssetMenu(fileName = "CharacterScanConfig", menuName = "Config/CharacterScanConfig", order = 1)]
    public class CharacterScanConfig : ScriptableObject
    {
        [Header("----- Scan Range -----")]
        public float OpenFogOfWarRange;
        public float ExploreRange;
        public float AttackRange;

        [Header("----- Scan Delay Time -----")]
        public float ScanEnemyDelay;
        public float ScanBuildingDelay;

        [Header("----- Scan Enemy Layer -----")]
        public LayerMask EnemyLayer;
        public LayerMask RewardBuildingLayer;
        public LayerMask EnemySpawnerLayer;
    }
}
