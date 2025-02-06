using Combat;
using Spawner;
using UnityEngine;

namespace Character
{
    public class EnemyCharacter : CharacterBase
    {
        public EnemyData EnemyData => Data as EnemyData;
        
        [SerializeField] private ProjectileSpawner projectileSpawner;

        private float cooldownTime;

        public override void Spawn(CharacterData data)
        {
            base.Spawn(data);
            
            
        }

        protected override string GetBrainType()
        {
            return "BehaviourGraph/EnemyBrain";
        }

        public override void Attack(Transform target)
        {
            if (cooldownTime > Time.time)
            {
                return;
            }
            
            var attackData = new AttackData()
            {
                Attacker = gameObject,
                Damage = EnemyData.Damage,
                AttackSpeed = EnemyData.AttackSpeed,
                AttackRange = EnemyData.AttackRange,
                Target = target != null ? target.gameObject : null,
                SpawnPos = transform.position,
                ProjectileConfig = EnemyData.ProjectileSO,
                Direction = target != null ? (target.position - transform.position).normalized : transform.forward,
                OnDestroyTarget = OnKillTargetSuccess
            };
            
            projectileSpawner.SpawnProjectile(attackData);
            cooldownTime = Time.time + EnemyData.Cooldown;
        }

        private void OnKillTargetSuccess()
        {
            
        }
    }
}
