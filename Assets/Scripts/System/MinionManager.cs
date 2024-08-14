using Character;
using Config;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class MinionManager : MonoBehaviour
    {
        public static MinionManager Instance;

        public CharacterScanConfig Config { get; private set; }

        private SimpleMinions minion;

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
            }

            Config = Resources.Load<CharacterScanConfig>("CharacterScanConfig");
        }

        [Button]
        private void CreateMinion() {
            var minionPrefab = Resources.Load<GameObject>("Minion");
            if (minionPrefab == null) {
                return;
            }

            if (minionPrefab.TryGetComponent(out minion) == false) {
                return;
            }

            Instantiate(minionPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
