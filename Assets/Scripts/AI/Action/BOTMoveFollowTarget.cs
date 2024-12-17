using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Character;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Move following the target")]
    public class BOTMoveFollowTarget : Action
    {
        [SerializeField] private SharedCharacterBase target;

        private CharacterBrain brain;
        private TaskStatus status;
        
        public override void OnAwake()
        {
            brain = GetComponent<CharacterBrain>();
        }

        public override void OnStart()
        {
            status = TaskStatus.Running;

            if (target.Value == null || !target.Value.CharacterHealth.IsAlive)
            {
                status = TaskStatus.Failure;
                return;
            }

            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget += OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget += OnFailMoveToTarget;

            StartCoroutine(MoveFollowTarget());
        }

        public override TaskStatus OnUpdate()
        {
            if (status == TaskStatus.Running && !target.Value.CharacterHealth.IsAlive)
            {
                status = TaskStatus.Failure;
            }
            
            return status;
        }

        public override void OnEnd()
        {
            StopAllCoroutines();
            
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget -= OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget -= OnFailMoveToTarget;
        }

        private IEnumerator MoveFollowTarget()
        {
            var waitingUpdate = new WaitForSeconds(0.5f);
            while (target.Value.CharacterHealth.IsAlive)
            {
                brain.LocalCharacter.CharacterMovement.MoveToTarget(target.Value.transform.position);
                yield return waitingUpdate;
            }
        }
        
        private void OnFailMoveToTarget()
        {
            status = TaskStatus.Failure;
        }

        private void OnCompleteMoveToTarget()
        {
            status = TaskStatus.Success;
        }
    }   
}
