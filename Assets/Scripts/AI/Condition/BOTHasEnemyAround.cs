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
        
        [Header("--------- Output ----------")]
        [SerializeField] private SharedCharacterBase enemy;

        public override TaskStatus OnUpdate()
        {
            return HasEnemyAround() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool HasEnemyAround()
        {
            var enemyAround = character.Value.CharacterBrain.GetEnemiesAround();
            if (enemyAround.Length == 0)
            {
                enemy.Value = null;
                return false;
            }

            enemy.Value = enemyAround.FindNearest(character.Value.transform.position);
            return true;
        }
    }   
}
