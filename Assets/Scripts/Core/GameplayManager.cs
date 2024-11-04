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

        private void OnDestroy()
        {
            resourcesManager.UnloadAllAssets();
        }

        private async Task PrepareCharacterResources()
        {
            foreach (C_Class charClass in Enum.GetValues(typeof(C_Class)))
            {
                await resourcesManager.LoadCharacterPrefabAsync(charClass.ToString());
            }

            Debug.Log("Done load character resources");
        }

        private async Task PrepareEqupimentResources()
        {
            foreach (EquipmentID equipmetID in Enum.GetValues(typeof(EquipmentID)))
            {
                if (equipmetID == EquipmentID.None)
                {
                    continue;
                }
                
                await resourcesManager.LoadCharacterPrefabAsync(equipmetID.ToString());
            }

            Debug.Log("Done load equpiment resources");
        }
    }   
}
