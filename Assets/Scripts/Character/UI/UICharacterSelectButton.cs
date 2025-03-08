using UnityEngine;
using UnityEngine.UI;
using Observer;
using Config;
using Inventory;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public class UICharacterSelectButton : MonoBehaviour
    {
        [SerializeField] private CharacterID characterID;
        [SerializeField] private Button myBtn;
        
        private AsyncOperationHandle<MinionConfig> configHandle;
        
        private void Start()
        {
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(OnButtonClick);
        }
        
        private async void OnButtonClick()
        {
            try
            {
                // Load the character config using Addressables
                string address = $"Config/Character/Minion/{characterID}";
                configHandle = Addressables.LoadAssetAsync<MinionConfig>(address);
                await configHandle.Task;
                
                if (configHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var config = configHandle.Result;
                    
                    if (config == null)
                    {
                        Debug.LogError($"Cannot find character {characterID} config");
                        return;
                    }
                    
                    EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory()
                    {
                        InventoryData = new InventoryData(
                            Mathf.Clamp(config.InventorySize.y, InventoryConstants.MIN_ROW, InventoryConstants.MAX_ROW), 
                            Mathf.Clamp(config.InventorySize.x, InventoryConstants.MIN_COLUMN, InventoryConstants.MAX_COLUMN),
                            config.ID
                        )
                    });
                    
                    // Release the handle when done with it
                    Addressables.Release(configHandle);
                }
                else
                {
                    Debug.LogError($"Failed to load character config: {address}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading character config: {ex.Message}");
            }
        }
        
        private void OnDestroy()
        {
            // Make sure to release the handle if it's still valid
            if (configHandle.IsValid())
            {
                Addressables.Release(configHandle);
            }
        }
    }   
}