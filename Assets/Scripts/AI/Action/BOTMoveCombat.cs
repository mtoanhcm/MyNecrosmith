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
            return base.OnUpdate();
        }

        private IEnumerator MoveAroundTarget()
        {
            var waitingUpdate = new WaitForSeconds(0.5f);
            
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
