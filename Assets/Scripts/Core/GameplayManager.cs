using System;
using System.Threading.Tasks;
using Character;
using Config;
using Observer;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnStartGame() { IsStart = true});
        }
    }   
}
