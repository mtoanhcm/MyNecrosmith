using Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace  Equipment
{
    public class SwordEquipment : EquipmentBase
    {
        [Button]
        public override void PerformAction(Transform target)
        {
            var attackData = new AttackData()
            {
                Attacker = Owner.gameObject,
                Damage = Data.Damage,
                AttackSpeed = Data.AttackSpeed,
                AttackRange = Data.AttackRange,
                Target = target != null ? target.gameObject : null,
                SpawnPos = transform.position,
                ProjectileConfig = projectileData,
                Direction = target != null ? (target.position - transform.position).normalized : Owner.transform.forward,
                OnDestroyTarget = OnKillTargetSuccess
            };
            
            projectileSpawner.SpawnProjectile(attackData);
        }

        public override void DeSpawn()
        {
            throw new System.NotImplementedException();
        }
    }   
}
