using Combat;
using Cysharp.Threading.Tasks;
using Gameplay.UI;
using Observer;
using SceneManage;
using System;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField]
        private GameMode gameMode;
        [SerializeField]
        private UIGameplayView uiGameplay;

        private SceneManager sceneManager;

        private async void Start()
        {
            EventManager.Instance.StartListening<EventData.OnRequestChangeScene>(OnSceneChangeCommandReceive);

            DamageReduction.Init();
            sceneManager = new SceneManager();

            gameMode.OnWinGameHandler = ShowWinGame;
            gameMode.OnLoseGameHandler = ShowGameOver;

            await UniTask.Delay(2 * 1000);
            
            StartGame();
        }

        private void StartGame()
        {
            Debug.Log("START GAME");
            EventManager.Instance.TriggerEvent(new EventData.OnGameplayStateChangeEvent() { 
                GameplayState = GameplayEventType.StartGame, 
                ChangeValue = true });

            gameMode.StartGameMode();
        }

        private void OnSceneChangeCommandReceive(EventData.OnRequestChangeScene data)
        {
            sceneManager.LoadSceneAsync(data.SceneType);
        }

        private void ShowGameOver() {
            _ = uiGameplay.ShowView<UIGameover>();

            EventManager.Instance.TriggerEvent(new EventData.OnGameplayStateChangeEvent()
            {
                GameplayState = GameplayEventType.Gameover,
                ChangeValue = true
            });
        }

        private void ShowWinGame() {
            _ = uiGameplay.ShowView<UIWin>();

            EventManager.Instance.TriggerEvent(new EventData.OnGameplayStateChangeEvent()
            {
                GameplayState = GameplayEventType.WinGame,
                ChangeValue = true
            });
        }
    }   
}
