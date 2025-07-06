using UnityEngine;
using UnityEngine.UI;
using Observer;
using Config;
using GameUtility;
using Minion.Inventory;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UIOpenMinionInventoryButton : MonoBehaviour
    {
        [SerializeField] private CharacterID characterID;
        
        private Button myBtn;

        private void Awake()
        {
            TryGetComponent(out myBtn);
        }
        
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
                Debug.LogError($"Cannot find minion {characterID} config");
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
