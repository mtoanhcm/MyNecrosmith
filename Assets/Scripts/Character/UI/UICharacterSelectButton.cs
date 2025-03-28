using UnityEngine;
using UnityEngine.UI;
using Observer;
using Config;
using GameUtility;
using Minion.Inventory;

namespace UI
{
    public class UICharacterSelectButton : MonoBehaviour
    {
        [SerializeField] private CharacterID characterID;
        [SerializeField] private Button myBtn;
        
        private void Start()
        {
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(LoadCharacterAsync);
        }

        private async void LoadCharacterAsync()
        {
            var path = $"Config/Character/Minion/{characterID}.asset";
            var config = await AddressableUtility.LoadAssetAsync<MinionConfig>(path);
            if (config == null)
            {
                Debug.LogError($"Cannot find character {characterID} config");
                return;
            }
                
            EventManager.Instance.TriggerEvent(new EventData.OpenMinionInventory()
            {
                InventoryData = new Minion.Inventory.Inventory(
                    Mathf.Clamp(config.InventorySize.y ,MinionInventoryParam.MIN_ROW, MinionInventoryParam.MAX_ROW), 
                    Mathf.Clamp(config.InventorySize.x, MinionInventoryParam.MIN_COLUMN, MinionInventoryParam.MAX_COLUMN),
                    config
                )
            });
        }
    }   
}
