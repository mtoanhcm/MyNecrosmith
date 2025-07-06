using System;
using UnityEngine;

namespace Gameplay {
    public abstract class GameMode : MonoBehaviour
    {
        public Action OnWinGameHandler { get; set; }
        public Action OnLoseGameHandler { get; set; }

        public float GameModePlayTime => gameModePlayTime;
        protected float gameModePlayTime;

        public bool IsGameStarted => isGameStarted;

        [SerializeField]
        protected float timeCheckGameModeCondition;

        private float tempTimeCheck;
        private bool isGameStarted;
        private bool isEndGame;

        private void Awake()
        {
            isEndGame = false;
            tempTimeCheck = 0f;

            InitGameMode();
        }

        private void Update()
        {
            if(isEndGame || !isGameStarted) {
                return;
            }

            if (tempTimeCheck > Time.time) {
                return;
            }

            tempTimeCheck = timeCheckGameModeCondition + Time.time;

            OnUpdate();

            if (WinCondition()) {
                isEndGame = true;
                OnWinGameHandler?.Invoke();
                return;
            }

            if(LoseCondition()) {
                isEndGame = true;
                OnLoseGameHandler?.Invoke();
                return;
            }
        }

        protected virtual void InitGameMode() { }
        public virtual void StartGameMode() {
            isGameStarted = true;
        }
        protected virtual void OnUpdate() { }
        protected abstract bool WinCondition();
        protected abstract bool LoseCondition();
    }
}
