using Config;
using GameUtility;
using InterfaceComp;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public CharacterData Data { get; private set; }
        public CharacterHealth CharacterHealth { get; private set; }
        public CharacterBrain CharacterBrain { get; private set; }
        public CharacterMovement CharacterMovement { get; private set; }

        public bool IsDebug;
        
        public virtual void Spawn(CharacterData data)
        {
            Data = data;
            SetupModel(data.ID);
            SetupHealth();
            SetupMovement();
            SetupAIBrain();
            
            Debug.Log("Active the brain");
            CharacterBrain.ActiveBrain();
        }

        public abstract void Attack();

        protected abstract void SetupModel(CharacterID id);

        protected virtual void SetupAIBrain()
        {
            if (CharacterBrain == null)
            {
                CharacterBrain = gameObject.AddComponent<CharacterBrain>();
            }
            
            CharacterBrain.Init(this, GetBrainType());
        }

        protected virtual void SetupHealth()
        {
            if (CharacterHealth == null)
            {
                CharacterHealth = gameObject.AddComponent<CharacterHealth>();
            }
            
            CharacterHealth.Init(this, OnCharacterDeath);
        }

        protected virtual void SetupMovement()
        {
            if (CharacterMovement == null)
            {
                CharacterMovement = gameObject.AddComponent<CharacterMovement>();
            }
            
            CharacterMovement.Init(this);
        }

        protected virtual void OnCharacterDeath()
        {
            CharacterBrain.DeActiveBrain();
        }
        
        protected abstract string GetBrainType();
        
    }   
}
