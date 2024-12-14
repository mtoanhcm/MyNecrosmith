using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        public UnityEvent OnCompleteMoveToTarget;
        public UnityEvent OnFailMoveToTarget;
        
        private const float REACHTHRESHOLD = 0.5f;
        private NavMeshAgent navAgent;

        private void Update()
        {
            if (navAgent == null || !navAgent.hasPath)
            {
                return;
            }

            if (IsAgentAtDestination())
            {
                OnCompleteMoveToTarget.Invoke();
                navAgent.ResetPath();
            }
        }
        
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
                
            }
            else
            {
                OnFailMoveToTarget?.Invoke();
            }
        }
        
        bool IsAgentAtDestination()
        {
            // Ensure the agent has a path and is not stopped
            if (!navAgent.pathPending && navAgent.remainingDistance <= REACHTHRESHOLD && !navAgent.hasPath)
            {
                return true;
            }
            return false;
        }
    }   
}
