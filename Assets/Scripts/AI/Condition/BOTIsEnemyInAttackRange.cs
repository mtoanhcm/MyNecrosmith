using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    [TaskCategory("AI_V1")]
    [TaskDescription("Check current enemy is in attack range")]
    public class BOTIsEnemyInAttackRange : Conditional
    {
        [Header("----- Input -----")]
        [SerializeField]
        private SharedGameObject targetEnemy;
    }
}
