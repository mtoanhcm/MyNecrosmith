using UnityEngine;
using UnityEngine.UI;
using Observer;
using Character;

namespace UI
{
    public class UICharacterSelectButton : MonoBehaviour
    {
        [SerializeField] private Button myBtn;

        [SerializeField] private int width;
        [SerializeField] private int height;
        
        private void Start()
        {
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(() =>
            {
                EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory()
                {
                    InventoryData = new Character.Inventory(Mathf.Clamp(width ,InventoryParam.MIN_ROW, InventoryParam.MAX_ROW), Mathf.Clamp(height, InventoryParam.MIN_COLUMN, InventoryParam.MAX_COLUMN))
                });
            });
        }
    }   
}
