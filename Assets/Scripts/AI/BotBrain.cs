using BehaviorDesigner.Runtime;
using Character;
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

        // ReSharper disable Unity.PerformanceAnalysis
        public void Init(BrainType brainType, CharacterBase myChar) {
            Character = myChar;
            InitBehaviourTree(brainType);
        }

        private void InitBehaviourTree(BrainType brainType)
        {
            if (behaviorTree == null || behaviorTree.ExternalBehavior == null)
            {
                behaviorTree = gameObject.AddComponent<BehaviorTree>();
                behaviorTree.StartWhenEnabled = false;
                behaviorTree.ExternalBehavior = Resources.Load<ExternalBehaviorTree>(brainType.ToString());
                behaviorTree.RestartWhenComplete = true;
            }

            if (behaviorTree.ExternalBehavior != null)
            {
                behaviorTree.EnableBehavior();
                behaviorTree.Start();
            }
        }
    }
}
