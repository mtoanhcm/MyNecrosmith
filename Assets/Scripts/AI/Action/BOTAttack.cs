using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Attack enemy")]
    public class BOTAttack : Action
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedCharacterBase targetEnemy;

        private BotBrain brain;
        private TaskStatus status;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override void OnStart()
        {
            status = TaskStatus.Running;
            StartCoroutine(ProgressAttack());
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }

        private IEnumerator ProgressAttack() {
            var enemyCharacter = targetEnemy.Value;
            var delayAttack = new WaitForSeconds(brain.Character.Stat.DelayAttackInSecond);
            while (enemyCharacter.IsAlive) {
                enemyCharacter.TakeDamage(brain.Character.Stat.Damage);
                yield return delayAttack;
            }

            status = TaskStatus.Failure;
        }
    }
}
