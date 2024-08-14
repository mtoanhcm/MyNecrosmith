using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check reward in detect range")]
    public class BOTHasRewardInRange : Conditional
    {
        [Header("----- Output -----")]
        [SerializeField]
        private SharedGameObject targetReward;
    }
}


