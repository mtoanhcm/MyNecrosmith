using BehaviorDesigner.Runtime;
using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    public enum BrainType {
        MinionBehaviour,
        EnemyBehaviour
    }

    public class BotBrain : MonoBehaviour
    {
        public CharacterBase Character { get; private set; }

        private BehaviorTree behaviorTree;

        public void Init(BrainType brainType, CharacterBase myChar) {
            Character = myChar;
            InitBehaviourTree(brainType);
        }

        private void InitBehaviourTree(BrainType brainType)
        {
            if (behaviorTree != null)
            {
                return;
            }

            behaviorTree = gameObject.AddComponent<BehaviorTree>();
            behaviorTree.StartWhenEnabled = false;
            behaviorTree.ExternalBehavior = Resources.Load<ExternalBehaviorTree>(brainType.ToString());
            behaviorTree.RestartWhenComplete = true;
            //behaviorTree.ResetValuesOnRestart = true;

            if (behaviorTree.ExternalBehavior != null)
            {
                behaviorTree.EnableBehavior();
                behaviorTree.Start();
            }
            else {
                Debug.LogError($"Cannot get {brainType} brain");
            }
        }
    }
}
