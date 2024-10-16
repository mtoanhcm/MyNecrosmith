using BehaviorDesigner.Runtime;
using UnityEngine;

namespace AI {

    public enum BrainType { 
        Minion,
        Enemy
    }

    public class BotBrain : MonoBehaviour
    {

        private BehaviorTree behaviorTree;

        public void Init(BrainType brainType) {
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

            behaviorTree.EnableBehavior();
            behaviorTree.Start();
        }
    }
}
