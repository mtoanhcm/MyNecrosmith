using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Claim the reward")]
    public class BOTClaimReward : Action
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedGameObject rewardObject;
    }
}
