using System;
using System.Threading.Tasks;
using Character;
using GameUtility;
using Observer;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private ResourcesManager resourcesManager;
        
        private async void Start()
        {
            await PrepareCharacterResources();
            
            EventManager.Instance.TriggerEvent(new EventData.OnStartGame() { IsStart = true});
        }

        private async Task PrepareCharacterResources()
        {
            foreach (C_Class charClass in Enum.GetValues(typeof(C_Class)))
            {
                await resourcesManager.LoadCharacterPrefabAsync(charClass.ToString());
            }

            Debug.Log("Done load character resouces");
        }
    }   
}
