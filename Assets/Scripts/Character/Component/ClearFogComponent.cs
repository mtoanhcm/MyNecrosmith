using Config;
using Map;
using UnityEngine;
using Observer;

namespace Character.Component {
    public class ClearFogComponent
    {
        private readonly float timeDelayCheckClearFog;
        private readonly float clearRadius;
        private float tempDelayTime;

        public ClearFogComponent(float radius, float timeDelay = 0) {
            timeDelayCheckClearFog = timeDelay;
            clearRadius = radius;
        }

        public void CheckClearFog(Vector3 charPosition)
        {
            if (timeDelayCheckClearFog > 0) {
                if (tempDelayTime > Time.time) {
                    return;
                }

                tempDelayTime = Time.time + timeDelayCheckClearFog;
            }

            EventManager.Instance.TriggerEvent(new EventData.OpenFogOfWarEvent() { Pos = charPosition, Radius = clearRadius });
        }
    }
}

