using AI;
using Character.Component;
using Map;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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

        public override void Spawn(CharacterID ID, Vector3 spawnPos, StatData baseStat)
        {
            transform.position = spawnPos;

            statComp = new(1, ID, baseStat);
            clearFogComp = new(statComp.ScanRange ,timeDelay: 0.25f);
            movementComp = new(transform, MapManager.Instance.groundManager.GroundMap, statComp.Speed);
            scanEnemyComp = new(statComp.ScanRange, LayerMask.GetMask("Enemy"), 0.5f);
            scanBuildingComp = new(statComp.ScanRange, LayerMask.GetMask("Building"), 1f);

            if (TryGetComponent(out brain)) {
                brain.Init(BrainType.MinionBehaviour, this);
            }

            scanEnemyComp.StartScan(transform);
            scanBuildingComp.StartScan(transform);
        }

        public override CharacterBase[] GetEnemyAround()
        {
            return scanEnemyComp.Enemies.ToArray();
        }

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
            CharacterPoolManager.Instance.ReturnMinionToPool(this);
        }
    }
}
