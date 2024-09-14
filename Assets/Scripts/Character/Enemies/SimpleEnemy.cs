using AI;
using Character.Component;
using Pool;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class SimpleEnemy : CharacterBase
    {
        private MovementComponent movementComp;
        private ScanEnemyComponent scanEnemyComp;
        private BotBrain brain;

        public override void Spawn(CharacterID id, Vector3 spawnPos, StatData baseStat)
        {
            transform.position = spawnPos;

            statComp = new StatComponent(1, id, baseStat);
            scanEnemyComp = new ScanEnemyComponent(statComp.ScanRange, LayerMask.GetMask("Minion"), 1f);
            movementComp = new MovementComponent(transform, statComp.Speed);
            if (TryGetComponent(out brain))
            {
                brain.Init(BrainType.EnemyBehaviour, this);
            }

            scanEnemyComp.StartScan(transform);
        }

        public override void OnUpdateExcute()
        {
            
        }

        public override CharacterBase[] GetEnemyAround()
        {
            return scanEnemyComp.Enemies.ToArray();
        }

        public override void MoveToTarget(Vector3 targetPos, UnityAction onEndPath)
        {
            movementComp.FindPath(transform.position, targetPos, onEndPath);
        }

        public override void StopMoving()
        {
            movementComp.StopMoving();
        }

        public override void UpdateStatData()
        {
            
        }

        public override void Death()
        {
            scanEnemyComp.StopScan();
            movementComp.StopMoving();
            CharacterPoolManager.Instance.ReturnEnemyToPool(this);
        }
    }
}
