using Combat;
using Config;
using Spawner;
using UnityEngine;

namespace  Equipment
{
    public class SwordEquipment : EquipmentBase
    {
        [Header("Projectile")]
        [SerializeField] private ProjectileID projectileID;
        [SerializeField] private ProjectileSpawner projectileSpawner;
        
        public override void PerformAction(Transform target)
        {
            var attackData = new AttackData()
            {
                //Attacker = Owner.gameObject,
            };
        }

        public override void DeSpawn()
        {
            throw new System.NotImplementedException();
        }
    }   
}
