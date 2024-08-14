using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Attack enemy")]
    public class BOTAttack : Action
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedGameObject targetEnemy;
    }
}
