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
        [SerializeField] private ResourcesManager resourcesManager;

        private void Awake()
        {
            resourcesManager = new ResourcesManager();
        }

        private async void Start()
        {
            await PrepareCharacterResources();
            await PrepareEqupimentResources();
            
            EventManager.Instance.TriggerEvent(new EventData.OnStartGame() { IsStart = true});
        }

        private async Task PrepareCharacterResources()
        {
            foreach (CharacterClass charClass in Enum.GetValues(typeof(CharacterClass)))
            {
                await resourcesManager.LoadCharacterPrefabAsync("Minion" ,charClass.ToString());
            }

            Debug.Log("Done load character resources");
        }

        private async Task PrepareEqupimentResources()
        {
            await resourcesManager.LoadEquipmentPrefabAsync(WeaponID.Sword.ToString());
            
            // foreach (WeaponID equipmetID in Enum.GetValues(typeof(WeaponID)))
            // {
            //     await resourcesManager.LoadEquipmentPrefabAsync(equipmetID.ToString());
            // }

            Debug.Log("Done load equipment resources");
        }
    }   
}
