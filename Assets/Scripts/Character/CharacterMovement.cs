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

        private IAstarAI ai;
        
        public void Init(CharacterBase character)
        {
            if(ai == null)
            {
                ai = GetComponent<IAstarAI>();
            }

            ai.maxSpeed = character.Data.RealMoveSpeed;
        }

        public void ToggleAutoRotation(bool isRotate)
        {
            ai.updateRotation = isRotate;
        }

        public void MoveToTarget(Vector3 target)
        {
            ai.isStopped = false;

            StopAllCoroutines();
            ai.destination = target;
            ai.SearchPath();
            StartCoroutine(CheckReachDestination());
        }

        public void StopMove()
        {
            StopAllCoroutines();
            OnMoving?.Invoke(0);
            ai.isStopped = true;
            OnCompleteMoveToTarget?.Invoke();
        }

        private IEnumerator CheckReachDestination()
        {
            var realVelocity = Vector3.zero;
            while (ai.pathPending || !ai.reachedEndOfPath)
            {
                realVelocity = transform.InverseTransformDirection(ai.velocity);
                realVelocity.y = 0;

                OnMoving?.Invoke(realVelocity.normalized.magnitude);
                yield return null;
            }

            StopMove();
        }
    }   
}
