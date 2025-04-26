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
        public UnityAction<float> OnMoving;

        private FollowerEntity followPath;
        
        public void Init(CharacterBase character)
        {
            followPath = GetComponent<FollowerEntity>();

            followPath.maxSpeed = character.Data.RealMoveSpeed;
        }

        public void MoveToTarget(Vector3 target)
        {
            followPath.SetDestination(target);
            StartCoroutine(CheckReachDestination());
        }

        public void StopMove()
        {
            OnMoving?.Invoke(0);
            OnCompleteMoveToTarget?.Invoke();
        }

        private IEnumerator CheckReachDestination()
        {
            var realVelocity = Vector3.zero;
            while (!followPath.reachedEndOfPath)
            {
                realVelocity = transform.InverseTransformDirection(followPath.velocity);
                realVelocity.y = 0;

                OnMoving?.Invoke(realVelocity.normalized.magnitude);
                yield return null;
            }

            StopMove();
        }
    }   
}
