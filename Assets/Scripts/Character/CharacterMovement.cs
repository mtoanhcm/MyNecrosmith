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
                StartCoroutine(CheckReachDestination());
            }
            else
            {
                OnFailMoveToTarget?.Invoke();
            }
        }

        private IEnumerator CheckReachDestination()
        {
            while (!IsAgentAtDestination())
            {
                yield return null;
            }
            
            navAgent.ResetPath();
            OnCompleteMoveToTarget.Invoke();
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
