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
        private UIGameplayView uiGameplay;

        private SceneManager sceneManager;

        private async void Start()
        {
            EventManager.Instance.StartListening<EventData.OnGameplayStateChangeEvent>(OnGameplayStateChange);
            EventManager.Instance.StartListening<EventData.OnRequestChangeScene>(OnSceneChangeCommandReceive);

            DamageReduction.Init();
            sceneManager = new SceneManager();

            await UniTask.Delay(2 * 1000);
            
            StartGame();
        }

        private void StartGame()
        {
            Debug.Log("START GAME");
            EventManager.Instance.TriggerEvent(new EventData.OnGameplayStateChangeEvent() { 
                GameplayState = GameplayEventType.StartGame, 
                ChangeValue = true });
        }

        private void OnGameplayStateChange(EventData.OnGameplayStateChangeEvent data)
        {
            if (data.GameplayState == GameplayEventType.EndGame && data.ChangeValue) {
                _ = uiGameplay.ShowView<UIGameover>();
            }
        }

        private void OnSceneChangeCommandReceive(EventData.OnRequestChangeScene data)
        {
            sceneManager.LoadSceneAsync(data.SceneType);
        }
    }   
}
