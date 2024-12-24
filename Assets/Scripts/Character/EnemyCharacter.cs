using Config;
using GameUtility;
using UnityEngine;

namespace Character
{
    public class EnemyCharacter : CharacterBase
    {
        protected override async void SetupModel(CharacterID id)
        {
            _ = await AddressableUtility.InstantiateAsync($"Model/Enemy/{id}.prefab", transform);
        }

        protected override string GetBrainType()
        {
            return "BehaviourGraph/EnemyBrain";
        }

        public override void Attack()
        {
            
        }
    }
}
