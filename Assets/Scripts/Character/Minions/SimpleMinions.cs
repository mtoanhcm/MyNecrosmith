using AI;
using Character.Component;
using Pool;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class SimpleMinions : CharacterBase
    {
        private ClearFogComponent clearFogComp;
        private MovementComponent movementComp;
        private ScanEnemyComponent scanEnemyComp;
        private ScanBuildingComponent scanBuildingComp;
        private BotBrain brain;

        public override void Spawn(CharacterID id, Vector3 spawnPos, StatData baseStat)
        {
            transform.position = spawnPos;

            statComp = new StatComponent(1, id, baseStat);
            clearFogComp = new ClearFogComponent(statComp.ScanRange ,timeDelay: 0.25f);
            movementComp = new MovementComponent(transform, statComp.Speed);
            scanEnemyComp = new ScanEnemyComponent(statComp.ScanRange, LayerMask.GetMask("Enemy"), 0.5f);
            scanBuildingComp = new ScanBuildingComponent(statComp.ScanRange, LayerMask.GetMask("Building"));

            if (TryGetComponent(out brain)) {
                brain.Init(BrainType.MinionBehaviour, this);
            }

            scanEnemyComp.StartScan(transform);
            scanBuildingComp.StartScan(transform);
            
            base.Spawn(id, spawnPos, baseStat);
        }

        public override CharacterBase[] GetEnemyAround()
        {
            return scanEnemyComp.Enemies.ToArray();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnUpdateExcute()
        {
            clearFogComp.CheckClearFog(transform.position);
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
            movementComp.StopMoving();
            scanBuildingComp.StopScan();
            scanBuildingComp.StopScan();
            CharacterPoolManager.Instance.ReturnMinionToPool(this);
        }
    }
}
