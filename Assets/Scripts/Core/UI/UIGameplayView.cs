using Inventory.UI;
using Observer;
using Core.UI;

namespace Gameplay.UI
{
    public class UIGameplayView : UISceneView
    {   
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
            var invenToryView = ShowView<UIInventoryPanel>();
            invenToryView.ShowMinionInventory(data.InventoryData);
        }
    }   
}
