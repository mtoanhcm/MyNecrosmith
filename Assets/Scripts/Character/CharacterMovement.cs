using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        public UnityAction OnCompleteMoveToTarget;
        public UnityAction OnFailMoveToTarget;
        public UnityAction<float> OnStartMoveToTarget;
        
        private const float REACHTHRESHOLD = 0.5f;
        private NavMeshAgent navAgent;
        
        public void Init(CharacterBase character)
        {
            if (navAgent == null)
            {
                navAgent = gameObject.AddComponent<NavMeshAgent>();
            }

            navAgent.speed = character.Data.RealMoveSpeed;
        }

        public void MoveToTarget(Vector3 target)
        {
            if (navAgent.SetDestination(target))
            {
                OnStartMoveToTarget?.Invoke(navAgent.speed);
                StartCoroutine(CheckReachDestination());
            }
            else
            {
                OnFailMoveToTarget?.Invoke();
            }
        }

        public void StopMove()
        {
            navAgent.ResetPath();
            OnCompleteMoveToTarget?.Invoke();
        }

        private IEnumerator CheckReachDestination()
        {
            while (!IsAgentAtDestination())
            {
                yield return null;
            }

            StopMove();
        }
        
        bool IsAgentAtDestination()
        {   
            // Ensure the agent has a path and is not stopped
            if (!navAgent.pathPending && navAgent.remainingDistance <= REACHTHRESHOLD)
            {
                return true;
            }
            
            return false;
        }
    }   
}
