using UnityEngine;
using BOT;
using Observer;
using System;

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

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnGameplayStateChangeEvent>(OnGameStageChanged);
        }

        public virtual void Spawn(CharacterData data, Vector3 spawnPos)
        {
            transform.position = spawnPos;

            Data = data;
            SetupHealth();
            SetupMovement();
            SetupAIBrain();
            SetupAnimation();
            
            CharacterBrain.ActiveBrain();

            gameObject.SetActive(true);
        }

        public abstract void Attack(Transform target);

        protected virtual void SetupAIBrain()
        {
            if (CharacterBrain == null)
            {
                CharacterBrain = gameObject.GetComponent<CharacterBrain>();
            }
            
            CharacterBrain.Init(this);
        }

        protected virtual void SetupHealth()
        {
            if (CharacterHealth == null)
            {
                CharacterHealth = gameObject.GetComponent<CharacterHealth>();
            }
            
            CharacterHealth.Init(this, OnCharacterDeath);
        }

        protected virtual void SetupMovement()
        {
            if (CharacterMovement == null)
            {
                CharacterMovement = gameObject.GetComponent<CharacterMovement>();
            }
            
            CharacterMovement.Init(this);
        }
        
        protected virtual void SetupAnimation()
        {
            if (CharacterAnimationController == null)
            {
                CharacterAnimationController = gameObject.GetComponent<CharacterAnimationController>();
            }
            
            CharacterAnimationController.Reset();

            CharacterMovement.OnMoving += CharacterAnimationController.PlayMoveAnimation;
        }

        protected virtual void OnCharacterDeath()
        {
            CharacterBrain.DeActiveBrain();
            CharacterMovement.StopMove();

            CharacterMovement.OnMoving -= CharacterAnimationController.PlayMoveAnimation;
        }

        private void OnGameStageChanged(EventData.OnGameplayStateChangeEvent data)
        {
            if((data.GameplayState == Gameplay.GameplayEventType.Gameover || data.GameplayState == Gameplay.GameplayEventType.WinGame) && data.ChangeValue)
            {
                //Stop all AI
                CharacterBrain.DeActiveBrain();
                
                //Stop all movement
                CharacterMovement.StopMove();
                
                //Reset animation
                CharacterAnimationController.Reset();

                return;
            }

            if (data.GameplayState == Gameplay.GameplayEventType.PauseGame) {
                CharacterMovement.PauseMove(data.ChangeValue);

                return;
            }
        }
    }   
}
