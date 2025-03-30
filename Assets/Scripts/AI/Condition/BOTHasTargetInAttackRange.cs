using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameUtility;
using UnityEngine;

namespace BOT
{
    
    [TaskCategory("BOT_V1")]
    [TaskDescription("Check target in attack range")]
    public class BOTHasTargetInAttackRange : Conditional
    {
        [SerializeField] private SharedCharacterBase character;
        
        [Header("---------- Input -----------")]
        [SerializeField] private SharedTransform target;

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }
            
            return character.Value.transform.IsWithinRadius(target.Value, character.Value.Data.AttackRange)
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }   
}
