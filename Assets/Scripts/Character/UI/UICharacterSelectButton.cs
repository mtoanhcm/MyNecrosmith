using UnityEngine;
using UnityEngine.UI;
using Observer;
using Character;
using Config;

namespace UI
{
    public class UICharacterSelectButton : MonoBehaviour
    {
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Button myBtn;
        
        private void Start()
        {
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(() =>
            {
                var config = Resources.Load<MinionConfig>($"Character/{characterClass}");
                if (config == null)
                {
                    Debug.LogError($"Cannot find character {characterClass} config");
                    return;
                }
                
                EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory()
                {
                    InventoryData = new Inventory(
                        Mathf.Clamp(config.InventorySize.y ,InventoryParam.MIN_ROW, InventoryParam.MAX_ROW), 
                        Mathf.Clamp(config.InventorySize.x, InventoryParam.MIN_COLUMN, InventoryParam.MAX_COLUMN),
                        config.Class
                        )
                });
            });
        }
    }   
}
