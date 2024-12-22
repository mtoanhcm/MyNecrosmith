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
        
        public void Init(CharacterBase character, string brainPath)
        {
            localCharacter = character;
            
            enemyScanner = new Scanner<CharacterBase>(
                100,
                () => localCharacter.transform.position,
                localCharacter.Data.ViewRadius,
                2,
                GetEnemyLayer(localCharacter.gameObject.layer),
                isDebug: character is MinionCharacter
            );
            
            if (behaviorTree != null)
            {
                behaviorTree.SetVariableValue("MainCharacter", localCharacter);
                return;
            }
            
            behaviorTree = gameObject.AddComponent<BehaviorTree>();
            behaviorTree.StartWhenEnabled = false;
            behaviorTree.RestartWhenComplete = true;
            behaviorTree.ExternalBehavior = Resources.Load<ExternalBehaviorTree>(brainPath);
            behaviorTree.SetVariableValue("MainCharacter", localCharacter);
        }

        public void ActiveBrain()
        {
            behaviorTree.EnableBehavior();
            enemyScanner.StartScanning();
        }

        public void DeActiveBrain()
        {
            behaviorTree.DisableBehavior();
            enemyScanner.StopScanning();
        }

        public CharacterBase[] GetEnemiesAround()
        {
            return enemyScanner.ObjectAround.ToArray();
        }

        private LayerMask GetEnemyLayer(LayerMask myLayer)
        {
            var enemyLayer = LayerMask.NameToLayer("Enemy");
            var minionLayer = LayerMask.NameToLayer("Minion");

            if (enemyLayer == -1 || minionLayer == -1)
            {
                Debug.LogError("Layer 'Enemy' or 'Minion' does not exist. Please add them in Tags and Layers.");
                return myLayer;
            }

            if (myLayer == enemyLayer)
            {
                return minionLayer;
            }
            else if (myLayer == minionLayer)
            {
                return enemyLayer;
            }

            return myLayer;
        }
    }   
}
