using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check enemy in detect range")]
    public class BOTHasEnemyInRange : Conditional
    {
        [Header("----- Output -----")]
        [SerializeField]
        private SharedGameObject targetEnemy;
    }
}
