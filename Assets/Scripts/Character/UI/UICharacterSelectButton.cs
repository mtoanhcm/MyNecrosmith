using UnityEngine;
using UnityEngine.UI;
using Observer;
using Character;
using Config;
using Inventory;
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
            myBtn.onClick.AddListener(() =>
            {
                var config = Resources.Load<MinionConfig>($"Character/Minion/{characterID}");
                if (config == null)
                {
                    Debug.LogError($"Cannot find character {characterID} config");
                    return;
                }
                
                EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory()
                {
                    InventoryData = new InventoryData(
                        Mathf.Clamp(config.InventorySize.y ,InventoryConstants.MIN_ROW, InventoryConstants.MAX_ROW), 
                        Mathf.Clamp(config.InventorySize.x, InventoryConstants.MIN_COLUMN, InventoryConstants.MAX_COLUMN),
                        config.ID
                        )
                });
            });
        }
    }   
}
