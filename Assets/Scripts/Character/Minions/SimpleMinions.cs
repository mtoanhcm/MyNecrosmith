using Character.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class SimpleMinions : CharacterBase
    {
        private ClearFogComponent clearFogComp;

        public override void InitComponent()
        {
            clearFogComp = new(timeDelay: 0.2f);
        }

        public override void OnUpdateExcute()
        {
            clearFogComp.CheckClearFog(transform.position);
        }
    }
}
