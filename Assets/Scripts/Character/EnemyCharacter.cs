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

        public override void Attack()
        {
            
        }
    }
}
