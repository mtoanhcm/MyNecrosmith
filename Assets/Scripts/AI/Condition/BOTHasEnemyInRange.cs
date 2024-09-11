using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Ultility;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check enemy in detect range")]
    public class BOTHasEnemyInRange : Conditional
    {
        [Header("----- Output -----")]
        [SerializeField]
        private SharedCharacterBase targetEnemy;

        [SerializeField] private SharedGameObject targetObject;

        private BotBrain brain;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override TaskStatus OnUpdate()
        {
            var enemies = brain.Character.GetEnemyAround();
            if (enemies.Length == 0) {
                targetEnemy.Value = null;
                return TaskStatus.Failure;
            }

            var enemy = enemies.FindNearest(transform);
            targetEnemy.Value = enemy;
            targetObject.Value = enemy != null ? enemy.gameObject : null;
            

            return TaskStatus.Success;
        }
    }
}
