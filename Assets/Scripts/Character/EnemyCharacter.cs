using Config;
using GameUtility;
using UnityEngine;

namespace Character
{
    public class EnemyCharacter : CharacterBase
    {

        protected override string GetBrainType()
        {
            return "BehaviourGraph/EnemyBrain";
        }

        public override void Attack()
        {
            
        }
    }
}
