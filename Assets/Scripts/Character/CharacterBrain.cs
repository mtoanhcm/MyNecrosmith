using BehaviorDesigner.Runtime;
using GameUtility;
using UnityEngine;

namespace Character
{
    public class CharacterBrain : MonoBehaviour
    {
        public bool IsCharacterAlive => localCharacter.CharacterHealth.IsAlive;
        public CharacterBase LocalCharacter => localCharacter;
        
        private BehaviorTree behaviorTree;
        private CharacterBase localCharacter;
        private Scanner<CharacterBase> enemyScanner;
        
        public void Init(CharacterBase character)
        {
            if (behaviorTree != null)
            {
                behaviorTree.SetVariableValue("MainCharacter", character);
                return;
            }
            
            behaviorTree = gameObject.AddComponent<BehaviorTree>();
            behaviorTree.StartWhenEnabled = false;
            behaviorTree.RestartWhenComplete = true;
            behaviorTree.ExternalBehavior = Resources.Load<ExternalBehaviorTree>("BehaviourGraph/MinionBrain");
            behaviorTree.SetVariableValue("MainCharacter", character);
            
            localCharacter = character;

            enemyScanner = new Scanner<CharacterBase>(
                100,
                () => localCharacter.transform.position,
                localCharacter.Data.ViewRadius,
                1,
                GetEnemyLayer(1<<localCharacter.gameObject.layer)
                );
        }

        public void ActiveBrain()
        {
            behaviorTree.EnableBehavior();
        }

        public CharacterBase[] GetEnemiesAround()
        {
            return enemyScanner.ObjectAround.ToArray();
        }

        private LayerMask GetEnemyLayer(LayerMask myLayer)
        {
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            int minionLayer = LayerMask.NameToLayer("Minion");

            if (enemyLayer == -1 || minionLayer == -1)
            {
                Debug.LogError($"Layer 'Enemy' or 'Minion' does not exist. Please add them in Tags and Layers.");
                return myLayer;
            }

            int enemyMask = 1 << enemyLayer;
            int minionMask = 1 << minionLayer;

            if (myLayer.value == enemyMask)
            {
                return minionMask;
            }
            else if (myLayer.value == minionMask)
            {
                return enemyMask;
            }

            return myLayer;
        }
    }   
}
