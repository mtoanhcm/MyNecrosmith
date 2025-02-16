using System;
using Observer;
using UnityEngine;

namespace Fog
{
    public class FogOfWarVisibleUnit : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnUnitRegisterOpenFogOfWar(){ Unit = transform});
        }

        private void OnDisable()
        {
            EventManager.Instance?.TriggerEvent(new EventData.OnUnitUnRegisterOpenFogOfWar() { Unit = transform });
        }
    }   
}
