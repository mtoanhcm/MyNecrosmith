using Gameplay;
using Minion.Inventory.UI;
using Player.Inventory.UI;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [SerializeField] private UIMinionInventoryView minionInventoryView;
        [SerializeField] private UIPlayerInventoryView playerInventoryView;
        
        public void ShowMinionInventory(Minion.Inventory.Inventory minionInventory)
        {
            gameObject.SetActive(true);
            
            minionInventoryView.OpenMinionInventory(minionInventory);
            playerInventoryView.OpenPlayerInventory(PlayerManager.Instance.Inventory);
        }

        public void CloseInventory()
        {
            gameObject.SetActive(false);
        }
    }   
}
