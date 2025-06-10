using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Character;
using GameUtility;
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

            combatRadius = character.Value.Data.AttackRange;

            brain.LocalCharacter.CharacterMovement.ToggleAutoRotation(false);

            MoveAroundTarget();
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

            brain.LocalCharacter.CharacterMovement.ToggleAutoRotation(true);

            brain.LocalCharacter.CharacterMovement.StopMove();
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget -= OnCompleteMoveToTarget;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget -= OnFailMoveToTarget;
        }

        private void MoveAroundTarget()
        {
            brain.LocalCharacter.CharacterMovement.MoveToTarget(target.Value.transform.position.GetRandomPositionAround(2, combatRadius));
        }
        
        private void OnFailMoveToTarget()
        {
            MoveAroundTarget();
        }

        private void OnCompleteMoveToTarget()
        {
            MoveAroundTarget();
        }
    }   
}
