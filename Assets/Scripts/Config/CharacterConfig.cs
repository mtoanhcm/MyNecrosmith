using Character;
using System;
using UnityEngine;

namespace Config {
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/CharacterConfig", order = 1)]
    public class CharacterConfig : ScriptableObject
    {
        [Serializable]
        public struct CharacterDataConfig {
            public CharacterID ID;
            public StatData StatData;
        }

        [SerializeField]
        private CharacterDataConfig[] datas;

        public bool TryGetCharacterData(CharacterID id, out StatData data) {
            data = default;

            var index = Array.FindIndex(datas, x => x.ID == id);
            if (index < 0) {
                return false;
            }

            data = datas[index].StatData;
            return true;
        }
    }
}
