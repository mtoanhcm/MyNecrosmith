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
        public bool isDebug;

        private TaskStatus status;

        public override void OnStart()
        {
            status = TaskStatus.Failure;
            if(isDebug)
            Debug.Log("A");
            
            var enemyAround = character.Value.CharacterBrain.GetEnemiesAround();
            Debug.Log($"{gameObject.name} detect enemy around {enemyAround.Length}");
            if (enemyAround.Length == 0)
            {
                return;
            }

            enemy.Value = enemyAround.FindNearest(character.Value.transform.position);
            status = enemy.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override TaskStatus OnUpdate()
        {
            if(isDebug)
                Debug.Log("A");
            
            return status;
        }
    }   
}
