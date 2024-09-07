using Character;
using Config;
using Pool;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class MinionManager : MonoBehaviour
    {
        public static MinionManager Instance;

        private CharacterConfig config;

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
            }

            config = Resources.Load<CharacterConfig>("CharacterConfig");
        }

        [Button]
        public void CreateMinion() {
            var minionPrefab = Resources.Load<SimpleMinions>("Minion");
            if (minionPrefab == null) {
                return;
            }

            var minion = CharacterPoolManager.Instance.SpawnMinion();

            config.TryGetCharacterData(CharacterID.SimpleMinion, out var statData);
            minion.Spawn(CharacterID.SimpleMinion, Vector3.zero, statData);
        }
    }
}
