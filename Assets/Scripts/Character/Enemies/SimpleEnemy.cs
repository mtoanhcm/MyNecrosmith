using AI;
using Character.Component;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class SimpleEnemy : CharacterBase
    {
        private MovementComponent movementComp;
        private BotBrain brain;

        public override void Spawn(CharacterID ID, StatData baseStat)
        {
            statComp = new(1, ID, baseStat);
            movementComp = new(transform, MapManager.Instance.groundManager.GroundMap, statComp.Speed);
            if (TryGetComponent(out brain)){
                brain.Init(BrainType.Enemy, this);
            }
        }

        public override void OnUpdateExcute()
        {
            
        }

        public override void MoveToTarget(Vector3 targetPos, UnityAction onEndPath)
        {
            movementComp.FindPath(transform.position, targetPos, onEndPath);
        }

        public override void UpdateStatData()
        {
            
        }

        public override void Death()
        {
            
        }
    }
}
