using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Ultility;
using UnityEngine;

namespace AI
{
    [TaskCategory("AI_V1")]
    [TaskDescription("Check target construct in attack range")]
    public class BOTIsConstructInAttackRange : Conditional
    {
        [Header("------ Input ------")]
        [SerializeField]
        private SharedBuildingBase targetBuilding;
        [SerializeField] 
        private SharedGameObject targetObject;

        private BotBrain brain;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetBuilding.Value == null || !targetBuilding.Value.IsAlive)
            {
                return TaskStatus.Failure;
            }
            
            return (targetBuilding.Value.transform.position - transform.position).sqrMagnitude > brain.Character.Stat.AttackRange * brain.Character.Stat.AttackRange ?
                TaskStatus.Failure : TaskStatus.Success;
        }
    }   
}
