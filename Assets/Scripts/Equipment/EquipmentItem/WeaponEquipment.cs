using Character;
using Combat;
using GameUtility;
using Spawner;
using UnityEngine;

namespace Equipment.Weapon
{
    public class WeaponEquipment : EquipmentBase
    {
        [Header("Projectile")]
        [SerializeField] private ProjectileSpawner projectileSpawner;
        
        private WeaponData weaponData;
        private float cooldown;

        public override void Init(CharacterBase owner, EquipmentData data, Vector3 spawnPosition)
        {
            base.Init(owner, data, spawnPosition);
            
            weaponData = data as WeaponData;
        }

        public override void PerformAction(Transform target)
        {
            if (cooldown > Time.time)
            {
                return;
            }
            
            var attackData = new AttackData()
            {
                Attacker = Owner.gameObject,
                Damage = weaponData.Damage,
                AttackSpeed = weaponData.AttackSpeed,
                AttackRange = weaponData.AttackRadius,
                Target = target != null ? target.gameObject : null,
                SpawnPos = transform.position,
                ProjectileConfig = weaponData.ProjectileSO,
                Direction = target != null ? (target.position - transform.position).normalized : Owner.transform.forward,
                OnDestroyTarget = OnKillTargetSuccess
            };
            
            projectileSpawner.SpawnProjectile(attackData);
            cooldown = Time.time + weaponData.Cooldown;
        }
    }
}
