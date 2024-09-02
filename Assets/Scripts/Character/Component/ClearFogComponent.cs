using Config;
using Map;
using UnityEngine;
using Observer;

namespace Character.Component {
    public class ClearFogComponent
    {
        private readonly CharacterScanConfig config;
        private float timeDelayCheckClearFog;

        private float tempDelayTime;

        public ClearFogComponent(float timeDelay = 0) {
            config = Resources.Load<CharacterScanConfig>("CharacterScanConfig");
            timeDelayCheckClearFog = timeDelay;
        }

        public void CheckClearFog(Vector3 charPosition)
        {
            if (timeDelayCheckClearFog > 0) {
                if (tempDelayTime > Time.time) {
                    return;
                }

                tempDelayTime = Time.time + timeDelayCheckClearFog;
            }

            EventManager.Instance.TriggerEvent(new EventData.OpenFogOfWarEvent() { Pos = charPosition, Radius = config.OpenFogOfWarRange });
        }
    }
}

