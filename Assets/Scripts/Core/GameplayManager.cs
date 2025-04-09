using System;
using System.Threading.Tasks;
using Character;
using Combat;
using Config;
using Cysharp.Threading.Tasks;
using Observer;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        private async void Start()
        {
            DamageReduction.Init();
            
            await UniTask.Delay(2 * 1000);
            
            StartGame();
        }

        private void StartGame()
        {
            Debug.Log("START GAME");
            EventManager.Instance.TriggerEvent(new EventData.OnStartGame() { IsStart = true});
        }
    }   
}
