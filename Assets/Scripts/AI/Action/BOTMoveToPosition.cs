using BehaviorDesigner.Runtime;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Character;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Character move to target position")]
    public class BOTMoveToPosition : Action
    {
        [SerializeField] private SharedVector3 targetMovePosition;

        private CharacterBrain brain;
        private TaskStatus status;

        public override void OnAwake()
        {
            brain = GetComponent<CharacterBrain>();
        }

        public override void OnStart()
        {
            status = TaskStatus.Running;
            
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget += OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget += OnFailMoveToTarget;
            
            brain.LocalCharacter.CharacterMovement.MoveToTarget(targetMovePosition.Value);
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }

        public override void OnEnd()
        {
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget += OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget += OnFailMoveToTarget;
        }

        private void OnCompleteMoveToTarget()
        {
            status = TaskStatus.Success;
        }

        private void OnFailMoveToTarget()
        {
            status = TaskStatus.Failure;
        }
    }   
}
