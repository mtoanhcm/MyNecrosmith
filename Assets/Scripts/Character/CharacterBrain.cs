using BehaviorDesigner.Runtime;
using BOT;
using Building;
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
        private Scanner<BuildingBase> enemyBuildingScanner;
        
        [SerializeField] private bool isDebug;
        
        public void Init(CharacterBase character, string brainPath)
        {
            localCharacter = character;
            
            enemyScanner = new Scanner<CharacterBase>(
                100,
                () => localCharacter.transform.position,
                localCharacter.Data.ViewRadius,
                2,
                GetEnemyLayer(localCharacter.gameObject.layer)
            );

            enemyBuildingScanner = new Scanner<BuildingBase>(
                100,
                () => localCharacter.transform.position,
                localCharacter.Data.ViewRadius,
                2,
                GetTargetBuildingLayer(localCharacter.gameObject.layer)
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
            enemyBuildingScanner.StartScanning();
        }

        public void DeActiveBrain()
        {
            behaviorTree.DisableBehavior();
            enemyScanner.StopScanning();
            enemyBuildingScanner.StopScanning();
        }

        public CharacterBase[] GetEnemiesAround()
        {
            return enemyScanner.ObjectAround.ToArray();
        }

        public BuildingBase[] GetTargetBuildingAround()
        {
            return enemyBuildingScanner.ObjectAround.ToArray();
        }

        private LayerMask GetEnemyLayer(LayerMask myLayer)
        {
            if (ObjectLayer.EnemyLayerIndex == -1 || ObjectLayer.MinionLayerIndex == -1)
            {
                Debug.LogError("Layer 'Enemy' or 'Minion' does not exist. Please add them in Tags and Layers.");
                return myLayer;
            }

            if (myLayer == ObjectLayer.EnemyLayerIndex)
            {
                return ObjectLayer.MinionLayerIndex;
            }
            else if (myLayer == ObjectLayer.MinionLayerIndex)
            {
                return ObjectLayer.EnemyLayerIndex;
            }

            return myLayer;
        }

        private LayerMask GetTargetBuildingLayer(LayerMask myLayer)
        {
            if (ObjectLayer.EnemyBuildingLayerIndex == -1 || ObjectLayer.MinionBuildingLayerIndex == -1)
            {
                Debug.LogError("Layer 'EnemyBuilding' or 'MinionBuilding' does not exist. Please add them in Tags and Layers.");
                return myLayer;
            }
            
            if (myLayer == ObjectLayer.EnemyLayerIndex)
            {
                return ObjectLayer.MinionBuildingLayerIndex;
            }
            else if (myLayer == ObjectLayer.MinionLayerIndex)
            {
                return ObjectLayer.EnemyBuildingLayerIndex;
            }

            return myLayer;
        }
    }   
}
