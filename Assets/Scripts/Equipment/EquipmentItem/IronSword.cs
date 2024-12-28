using Character;
using Combat;
using Config;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Equipment
{
    public class IronSword : EquipmentBase
    {

        public override void Init(CharacterBase owner ,EquipmentData data, Vector3 spawnPosition)
        {
            base.Init(owner ,data, spawnPosition);
        }

        [Button]
        public override void PerformAction(Transform target)
        {
            
        }

        public override void DeSpawn()
        {
            
        }
    }   
}
