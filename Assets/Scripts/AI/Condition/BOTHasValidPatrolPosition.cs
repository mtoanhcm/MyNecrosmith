using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check has valid position to patrol")]
    public class BOTHasValidPatrolPosition : Conditional
    {
        [Header("----- Output -----")]
        [SerializeField]
        private SharedVector3 targetPosition;

        private BotBrain brain;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Failure;
        }
    }
}
