using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check current enemy is in attack range")]
    public class BOTIsEnemyInAttackRange : Conditional
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedCharacterBase targetEnemy;

        public override TaskStatus OnUpdate()
        {
            if (targetEnemy == null) { 
                return TaskStatus.Failure;
            }

            return (targetEnemy.Value.transform.position - transform.position).sqrMagnitude > 1.5f * 1.5f ?
                TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
