using BehaviorDesigner.Runtime.Tasks;
using GameUtility;
using UnityEngine;

namespace BOT
{
    
    [TaskCategory("BOT_V1")]
    [TaskDescription("Check enemy in attack range")]
    public class BOTHasEnemyInAttackRange : Conditional
    {
        [SerializeField] private SharedCharacterBase character;
        
        [Header("---------- Input -----------")]
        [SerializeField] private SharedCharacterBase enemy;

        public override TaskStatus OnUpdate()
        {
            return character.Value.transform.IsWithinRadius(enemy.Value.transform, character.Value.Data.AttackRange)
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }   
}
