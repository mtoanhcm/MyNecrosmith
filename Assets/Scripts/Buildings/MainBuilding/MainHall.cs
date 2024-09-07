using Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public class MainHall : BuildingBase
    {
        public override void Claimp()
        {
            
        }

        public override void PlayActivation()
        {
            
        }

        public override void OnSubInit()
        {
            EventManager.Instance.TriggerEvent(new EventData.OpenFogOfWarEvent() { Pos = transform.position, Radius = 10 });
        }

        public override void TakeDamage(float damage)
        {
            
        }

        public override void Explose()
        {

        }
    }
}
