using AI;
using Character.Component;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class SimpleEnemy : CharacterBase
    {
        private MovementComponent movementComp;
        private ScanEnemyComponent scanEnemyComp;
        private BotBrain brain;

        public override void Spawn(CharacterID ID, Vector3 spawnPos, StatData baseStat)
        {
            transform.position = spawnPos;

            statComp = new(1, ID, baseStat);
            scanEnemyComp = new(statComp.ScanRange, LayerMask.GetMask("Minion"), 1f);
            movementComp = new(transform, MapManager.Instance.groundManager.GroundMap, statComp.Speed);
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

        public override void UpdateStatData()
        {
            
        }

        public override void Death()
        {
            
        }
    }
}
