using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("BOT attack target building")]
    public class BOTAttackBuilding : Action
    {
        [SerializeField] private SharedCharacterBase character;
        [SerializeField] private SharedBuildingBase targetBuilding;
        
        private const float ROTATE_SPEED = 5f;
        
        public override void OnStart()
        {
            StartCoroutine(FaceToTargetBuilding());
            character.Value.CharacterMovement.StopMove();
        }
        
        public override void OnEnd()
        {
            StopAllCoroutines();
        }
        
        public override TaskStatus OnUpdate()
        {
            if(!IsTargetBuildingValid())
            {
                return TaskStatus.Failure;
            }
            
            character.Value.Attack(targetBuilding.Value.transform);
            return TaskStatus.Running;
        }
        
        private IEnumerator FaceToTargetBuilding()
        {
            var characterTrans = character.Value.transform;
            var targetTrans = targetBuilding.Value.transform;

            while (IsTargetBuildingValid())
            {
                var direction = targetTrans.position - characterTrans.position;
                if (direction == Vector3.zero)
                {
                    yield return null;
                    continue;
                }
                
                // Calculate the desired rotation towards the target
                var targetRotation = Quaternion.LookRotation(direction);

                characterTrans.rotation = Quaternion.Slerp(characterTrans.rotation, targetRotation,
                    ROTATE_SPEED * Time.deltaTime);
                
                yield return null;
            }
        }
        
        private bool IsTargetBuildingValid()
        {
            return targetBuilding.Value != null && targetBuilding.Value.Health.IsAlive;
        }
    }   
}
