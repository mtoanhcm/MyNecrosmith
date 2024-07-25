using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check current reward is in claim range")]
    public class BOTIsRewardInClaimRange : Conditional
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedGameObject targetReward;
    }
}
