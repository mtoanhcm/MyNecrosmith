using Inventory.UI;
using Observer;
using UnityEngine;

namespace Training.UI
{
    public class UITrainingView : MonoBehaviour
    {
        [SerializeField] private UIInventoryPanel inventoryPanel;
        
        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OpenMinionInventory>(OnGetCharacterInventoryData);
        }

        private void OnDisable()
        {
            if (EventManager.HasInstance)
            {
                EventManager.Instance.StopListening<EventData.OpenMinionInventory>(OnGetCharacterInventoryData);
            }
        }
        
        private void OnGetCharacterInventoryData(EventData.OpenMinionInventory data)
        {
            inventoryPanel.ShowMinionInventory(data.InventoryData);
        }
    }   
}
