using Character.Component;
using Map;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class SimpleMinions : CharacterBase
    {
        private ClearFogComponent clearFogComp;
        private MovementComponent movementComp;

        public override void InitComponent()
        {
            clearFogComp = new(timeDelay: 0.2f);
            movementComp = new(transform, MapManager.Instance.groundManager.GroundMap, 5f);
        }

        public override void OnUpdateExcute()
        {
            clearFogComp.CheckClearFog(transform.position);
        }

        public override void MoveToTarget(Vector3 targetPos, UnityAction onEndPath)
        {
            movementComp.FindPath(transform.position, targetPos, onEndPath);
        }
    }
}
