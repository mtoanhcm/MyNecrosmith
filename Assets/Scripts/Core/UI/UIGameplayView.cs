using UnityEngine;
using Observer;

namespace UI
{
    public class UIGameplayView : MonoBehaviour
    {
        [SerializeField] private UICharacterListView characterListView;
        [SerializeField] private UIInventoryView inventoryView;
        
        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.OpenCharacterInventory>(OnGetCharacterInventoryData);
        }

        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OpenCharacterInventory>(OnGetCharacterInventoryData);
        }

        private void OnGetCharacterInventoryData(EventData.OpenCharacterInventory data)
        {
            inventoryView.OpenCharacterInventory(data.InventoryData);
        }
    }   
}
