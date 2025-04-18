using Pathfinding;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Character
{
    [RequireComponent(typeof(FollowerEntity))]
    public class CharacterMovement : MonoBehaviour
    {
        public UnityAction OnCompleteMoveToTarget;
        public UnityAction OnFailMoveToTarget;
        public UnityAction<float> OnStartMoveToTarget;

        private FollowerEntity followPath;
        
        public void Init(CharacterBase character)
        {
            followPath = GetComponent<FollowerEntity>();

            followPath.maxSpeed = character.Data.RealMoveSpeed;
        }

        public void MoveToTarget(Vector3 target)
        {
            followPath.SetDestination(target);
            OnStartMoveToTarget?.Invoke(followPath.maxSpeed);
            StartCoroutine(CheckReachDestination());
        }

        public void StopMove()
        {
            OnCompleteMoveToTarget?.Invoke();
        }

        private IEnumerator CheckReachDestination()
        {
            while (!followPath.reachedEndOfPath)
            {
                yield return null;
            }

            StopMove();
        }
    }   
}
