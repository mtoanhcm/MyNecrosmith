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

        private BotBrain brain;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override TaskStatus OnUpdate()
        {
            var enemies = brain.Character.GetEnemyAround();
            if (enemies.Length == 0) { 
                return TaskStatus.Failure;
            }

            targetEnemy.Value = enemies.FindNearest(transform);

            return TaskStatus.Success;
        }
    }
}
