using Config;
using InterfaceComp;
using Sirenix.OdinInspector;
using Unity.Behavior;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public CharacterData Data { get;private set; }
        
        private CharacterHealth characterHealth;
        private BehaviorGraphAgent behaviorAgent;

        private void Awake()
        {
            characterHealth = gameObject.AddComponent<CharacterHealth>();
            characterHealth.Init(this);

            behaviorAgent = gameObject.AddComponent<BehaviorGraphAgent>();
            behaviorAgent.Graph = Resources.Load<BehaviorGraph>("Graph/SimpleBrain");
        }
        
        public virtual void Spawn(CharacterData data)
        {
            Data = data;
            SetupModel(data.ID);
        }

        protected abstract void SetupModel(CharacterID id);

        [Button]
        private void Test()
        {
            var aaa = GetComponent<IHealth>();
            Debug.Log(aaa);
        }
    }   
}
