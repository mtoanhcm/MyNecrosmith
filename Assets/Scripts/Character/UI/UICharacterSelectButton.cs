using UnityEngine;
using UnityEngine.UI;
using Observer;
using Character;
using Config;
using GameUtility;
using UnityEngine.Serialization;

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
                
            EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory()
            {
                InventoryData = new Inventory(
                    Mathf.Clamp(config.InventorySize.y ,InventoryParam.MIN_ROW, InventoryParam.MAX_ROW), 
                    Mathf.Clamp(config.InventorySize.x, InventoryParam.MIN_COLUMN, InventoryParam.MAX_COLUMN),
                    config.ID
                )
            });
        }
    }   
}
