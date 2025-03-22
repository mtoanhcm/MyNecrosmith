using Config;
using Gameplay;
using Minion.Inventory.UI;
using Observer;
using Player.Inventory.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [SerializeField] private UIMinionInventoryView minionInventoryView;
        [SerializeField] private UIPlayerInventoryView playerInventoryView;
        [SerializeField] private Button spawnBtn;
        
        public void ShowMinionInventory(Minion.Inventory.Inventory minionInventory)
        {
            gameObject.SetActive(true);
            
            minionInventoryView.OpenMinionInventory(minionInventory);
            playerInventoryView.OpenPlayerInventory(PlayerManager.Instance.Inventory);
            
            spawnBtn.onClick.RemoveAllListeners();
            spawnBtn.onClick.AddListener(() =>  OnSpawnMinion(minionInventory.MinionConfig));
        }

        public void CloseInventory()
        {
            gameObject.SetActive(false);
        }

        private void OnSpawnMinion(MinionConfig config)
        {
            if (!minionInventoryView.TryToGetInventoryItemsForSpawnMinion(out var equipmentData))
            {
                return;
            }
            
            EventManager.Instance.TriggerEvent(new EventData.OnPrepareEquipmentForSpawnMinion() { Equipment = equipmentData, MinionConfig = config });
            minionInventoryView.ClearInventory();
            CloseInventory();
        }
    }   
}
