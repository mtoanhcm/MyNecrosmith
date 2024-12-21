using BehaviorDesigner.Runtime.Tasks;
using Character;
using GameUtility;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Character move patrol on map")]
    public class BOTMovePatrol : Action
    {
        [SerializeField]
        private SharedCharacterBase character;

        private TaskStatus status;
        private CharacterBrain brain;

        public override void OnStart()
        {
            status = TaskStatus.Running;

            brain = character.Value.CharacterBrain;
            
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget += OnCharacterMoveComplete;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget += OnCharacterCanNotMoveToTargetPosition;

            PatrolToNextPoint();
        }

        public override void OnEnd()
        {
            brain.LocalCharacter.CharacterMovement.OnCompleteMoveToTarget -= OnCharacterMoveComplete;
            brain.LocalCharacter.CharacterMovement.OnFailMoveToTarget -= OnCharacterCanNotMoveToTargetPosition;
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }
        
        private void PatrolToNextPoint()
        {
            brain.LocalCharacter.CharacterMovement.MoveToTarget(brain.LocalCharacter.transform.position.GetRandomNavmeshPositionAround(5f , brain.LocalCharacter.Data.ViewRadius));
        }
        
        private void OnCharacterCanNotMoveToTargetPosition()
        {
            PatrolToNextPoint();
        }

        private void OnCharacterMoveComplete()
        {
            PatrolToNextPoint();
        }
    }   
}
