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
    }
}
