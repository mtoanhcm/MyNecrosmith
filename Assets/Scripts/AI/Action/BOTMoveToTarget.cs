using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using Ultility;
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

            Vector3 targetMovePos = targetObject.Value == null ?
                targetPosition.Value :
                targetObject.Value.transform.position.GetRandomPositionAround(1f);

            brain.Character.MoveToTarget(targetMovePos, OnCompleteMove);

            if (targetObject.Value != null) {
                StartCoroutine(DelayChaseTargetObject());
            }
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }

        private IEnumerator DelayChaseTargetObject() {
            var waitForSecond = new WaitForSeconds(2f);
            while (targetObject.Value != null) {
                yield return waitForSecond;

                brain.Character.MoveToTarget(targetObject.Value.transform.position.GetRandomPositionAround(1f), OnCompleteMove);
            }
        }

        private void OnCompleteMove() {
            status = TaskStatus.Success;
        }
    }
}
