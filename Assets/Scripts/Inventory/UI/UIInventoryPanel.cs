using System;
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
        [SerializeField] private InventoryItemDragHandle dragHandle;
        [SerializeField] private Button spawnBtn;
        [SerializeField] private Button closeBtn;

        private void Awake()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(CloseInventory);
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OnPlayerInventoryChanged>(OnPlayerInventoryChanged);
        }

        private void OnDisable()
        {
            EventManager.Instance.StopListening<EventData.OnPlayerInventoryChanged>(OnPlayerInventoryChanged);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseInventory();
            }
        }

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
            dragHandle.ResetDraggedItem(out var draggedEquipmentData);
            if (draggedEquipmentData != null)
            {
                PlayerManager.Instance.Inventory.AddEquipmentToStorage(draggedEquipmentData);
            }
            
            minionInventoryView.ClearInventory(out var minionHolderEquipment);
            for (var i = 0; i < minionHolderEquipment.Length; i++)
            {
                PlayerManager.Instance.Inventory.AddEquipmentToStorage(minionHolderEquipment[i]);
            }
            
            gameObject.SetActive(false);
        }

        private void OnPlayerInventoryChanged(EventData.OnPlayerInventoryChanged data)
        {
            if (!data.HasChange)
            {
                return;
            }
            
            playerInventoryView.OpenPlayerInventory(PlayerManager.Instance.Inventory);
        }
        
        private void OnSpawnMinion(MinionConfig config)
        {
            if (!minionInventoryView.TryToGetInventoryItemsForSpawnMinion(out var equipmentData))
            {
                return;
            }
            
            EventManager.Instance.TriggerEvent(new EventData.OnPrepareEquipmentForSpawnMinion() { Equipment = equipmentData, MinionConfig = config });
            minionInventoryView.ClearInventory(out _);
            CloseInventory();
        }
    }   
}
