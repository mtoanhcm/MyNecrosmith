using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Move to target object or target position")]
    public class BOTMoveToTarget : Action
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedGameObject targetObject;//Piority move to target object
        [SerializeField]
        private SharedVector3 targetPosition;

        private TaskStatus status;
        private BotBrain brain;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override void OnStart()
        {
            status = TaskStatus.Running;

            if (targetObject.Value == null) {
                brain.Character.MoveToTarget(targetPosition.Value, OnCompleteMove);
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetObject.Value != null) { 
                brain.Character.MoveToTarget(targetObject.Value.transform.position, OnCompleteMove);
            } else
            {
                status = TaskStatus.Success;
            }

            return status;
        }

        private void OnCompleteMove() {
            status = TaskStatus.Success;
        }
    }
}
