using Inventory.UI;
using Minion.Inventory.UI;
using UnityEngine;
using Observer;
using UI;
using UnityEngine.Serialization;

namespace Gameplay.UI
{
    public class UIGameplayView : MonoBehaviour
    {
        [SerializeField] private UICharacterListView characterListView;
        [SerializeField] private UIInventoryPanel inventoryPanel;
        
        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.OpenMinionInventory>(OnGetCharacterInventoryData);
        }

        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OpenMinionInventory>(OnGetCharacterInventoryData);
        }

        private void OnGetCharacterInventoryData(EventData.OpenMinionInventory data)
        {
            inventoryPanel.ShowMinionInventory(data.InventoryData);
        }
    }   
}
