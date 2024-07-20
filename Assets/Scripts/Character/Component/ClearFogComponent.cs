using Config;
using Map;
using UnityEngine;

namespace Character.Component {
    public class ClearFogComponent
    {
        private readonly CharacterScanConfig config;

        public ClearFogComponent() {
            config = Resources.Load<CharacterScanConfig>("CharacterScanConfig");
        }

        private void CheckClearFog(Vector3 charPosition)
        {
            MapManager.Instance.OnCheckClearFog(charPosition, config.OpenFogOfWarRange);
        }
    }
}

