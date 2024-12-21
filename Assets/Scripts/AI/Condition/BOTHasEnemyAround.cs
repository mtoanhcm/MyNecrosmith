using BehaviorDesigner.Runtime.Tasks;
using GameUtility;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Check has enemy around")]
    public class BOTHasEnemyAround : Conditional
    {
        [SerializeField] private SharedCharacterBase character;
        [SerializeField] private SharedCharacterBase enemy;

        private TaskStatus status;

        public override void OnStart()
        {
            status = TaskStatus.Running;

            var enemyAround = character.Value.CharacterBrain.GetEnemiesAround();
            if (enemyAround.Length == 0)
            {
                status = TaskStatus.Failure;
                return;
            }

            enemy.Value = enemyAround.FindNearest(character.Value.transform.position);
            status = enemy.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }
    }   
}
