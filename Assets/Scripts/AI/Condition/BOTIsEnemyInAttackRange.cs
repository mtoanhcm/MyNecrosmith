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

        private BotBrain brain;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetEnemy == null) { 
                return TaskStatus.Failure;
            }

            return (targetEnemy.Value.transform.position - transform.position).sqrMagnitude > brain.Character.Stat.AttackRange * brain.Character.Stat.AttackRange ?
                TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
