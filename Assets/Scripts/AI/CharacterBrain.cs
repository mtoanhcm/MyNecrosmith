using BehaviorDesigner.Runtime;
using Building;
using GameUtility;
using UnityEngine;
using Character;
using UnityEngine.SceneManagement;
using Pathfinding;

namespace BOT
{
    public class CharacterBrain : MonoBehaviour
    {
        public bool IsCharacterAlive => localCharacter.CharacterHealth.IsAlive;
        public CharacterBase LocalCharacter => localCharacter;
        public IAstarAI AI => ai;

        private BehaviorTree behaviorTree;
        private CharacterBase localCharacter;
        private Scanner<CharacterBase> enemyScanner;
        private Scanner<BuildingBase> enemyBuildingScanner;
        private IAstarAI ai;

        [SerializeField] private BrainType brainType;
        [SerializeField] private bool isDebug;
        
        public void Init(CharacterBase character)
        {
            localCharacter = character;
            ai = GetComponent<IAstarAI>();

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
            behaviorTree.ExternalBehavior = Resources.Load<ExternalBehaviorTree>($"BehaviourGraph/{GetBrainType()}");
            behaviorTree.SetVariableValue("MainCharacter", localCharacter);
        }

        public void ActiveBrain()
        {
            behaviorTree.EnableBehavior();
            _ = enemyScanner.StartScanning();
            _ = enemyBuildingScanner.StartScanning();
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

        private BrainType GetBrainType()
        {
            if (SceneManager.GetActiveScene().name.Contains("Training"))
            {
                return BrainType.TrainingBrain;
            }

            return brainType;
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
