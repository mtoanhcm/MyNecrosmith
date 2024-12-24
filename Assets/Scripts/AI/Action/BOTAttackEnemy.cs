using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("BOT attack target enemy")]
    public class BOTAttackEnemy : Action
    {
        [SerializeField] private SharedCharacterBase character;
        [SerializeField] private SharedCharacterBase enemy;

        private const float ROTATE_SPEED = 5f;
        
        public override void OnStart()
        {
            StartCoroutine(FaceToTargetEnemy());
        }

        public override void OnEnd()
        {
            StopAllCoroutines();
        }

        public override TaskStatus OnUpdate()
        {
            if(!IsEnemyValid())
            {
                return TaskStatus.Failure;
            }
            
            character.Value.Attack();
            return TaskStatus.Running;
        }

        private bool IsEnemyValid()
        {
            return enemy.Value != null && enemy.Value.CharacterHealth.IsAlive;
        }
        
        private IEnumerator FaceToTargetEnemy()
        {
            var characterTrans = character.Value.transform;
            var enemyTrans = enemy.Value.transform;

            while (IsEnemyValid())
            {
                var direction = enemyTrans.position - characterTrans.position;
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
    }   
}
