using System;
using System.Runtime.CompilerServices;
using Config;
using GameUtility;
using InterfaceComp;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    [RequireComponent(typeof(CharacterHealth), typeof(CharacterBrain), typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterAnimationController))]
    public abstract class CharacterBase : MonoBehaviour
    {
        [field: SerializeField]
        public CharacterData Data { get; private set; }
        public CharacterHealth CharacterHealth { get; private set; }
        public CharacterBrain CharacterBrain { get; private set; }
        public CharacterMovement CharacterMovement { get; private set; }
        
        public CharacterAnimationController CharacterAnimationController { get; private set; }

        public bool IsDebug;

        public virtual void Spawn(CharacterData data)
        {
            Data = data;
            SetupHealth();
            SetupMovement();
            SetupAIBrain();
            SetupAnimation();
            
            Debug.Log($"Character {name} Active the brain");
            CharacterBrain.ActiveBrain();
        }

        public abstract void Attack();

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
        
        protected virtual void SetupAnimation()
        {
            if (CharacterAnimationController == null)
            {
                CharacterAnimationController = gameObject.AddComponent<CharacterAnimationController>();
            }
            
            CharacterAnimationController.Reset();

            CharacterMovement.OnStartMoveToTarget += CharacterAnimationController.PlayMoveAnimation;
        }

        protected virtual void OnCharacterDeath()
        {
            CharacterBrain.DeActiveBrain();
            
            CharacterMovement.OnStartMoveToTarget -= CharacterAnimationController.PlayMoveAnimation;
        }
        
        protected abstract string GetBrainType();
        
    }   
}
