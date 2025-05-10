using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Character;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("BOT attack target enemy")]
    public class BOTMoveCombat : Action
    {
        [SerializeField] private SharedCharacterBase character;
        [SerializeField] private SharedCharacterBase target;
        
        private CharacterBrain brain;
        private TaskStatus status;
        private float combatRadius;

        public override void OnStart()
        {
            status = TaskStatus.Running;
            
            if (target.Value == null || !target.Value.CharacterHealth.IsAlive)
            {
                status = TaskStatus.Failure;
                return;
            }
            
            brain = character.Value.CharacterBrain;
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget += OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget += OnFailMoveToTarget;

            StartCoroutine(MoveAroundTarget());
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }

        public override void OnEnd()
        {
            StopAllCoroutines();

            if (brain == null)
            {
                return;
            }

            brain.LocalCharacter.CharacterMovement.StopMove();
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget -= OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget -= OnFailMoveToTarget;
        }

        private IEnumerator MoveAroundTarget()
        {
            var waitingUpdate = new WaitForSeconds(0.5f);
            float desiredDistance = 5.0f; // Desired distance to maintain from the target
            while (target.Value != null && target.Value.CharacterHealth.IsAlive)
            {
                Vector3 directionToTarget = (target.Value.transform.position - brain.LocalCharacter.transform.position).normalized;
                Vector3 targetPosition = target.Value.transform.position - directionToTarget * desiredDistance;

                brain.LocalCharacter.CharacterMovement.MoveToTarget(targetPosition);
                yield return waitingUpdate;
            }

            brain.LocalCharacter.CharacterMovement.StopMove();
            status = TaskStatus.Failure;
        }
        
        private void OnFailMoveToTarget()
        {
            status = TaskStatus.Failure;
        }

        private void OnCompleteMoveToTarget()
        {
            status = TaskStatus.Success;
        }

        Vector3 PickRandomPoint()
        {
            var point = Random.insideUnitSphere * combatRadius ;

            point.y = 0;
            point += brain.AI.position;
            return point;
        }
    }   
}
