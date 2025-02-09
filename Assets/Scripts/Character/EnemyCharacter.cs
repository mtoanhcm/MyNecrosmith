using Combat;
using Observer;
using Spawner;
using UnityEngine;

namespace Character
{
    public class EnemyCharacter : CharacterBase
    {
        public EnemyData EnemyData => Data as EnemyData;
        
        [SerializeField] private ProjectileSpawner projectileSpawner;

        private float cooldownTime;

        protected override void OnCharacterDeath()
        {
            base.OnCharacterDeath();
            
            EventManager.Instance.TriggerEvent(new EventData.OnEnemyDeath(){ Enemy = this});
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
                SpawnPos = projectileSpawner.transform.position,
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
