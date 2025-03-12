using System;
using System.Threading.Tasks;
using Inventory.UI;
using Observer;
using UnityEngine;

namespace Pool
{
    public class UIInventoryItemPool : MonoBehaviour
    {
        [SerializeField] private int initialPoolSize = 10;
        private ObjectPool<UIInventoryItem> itemPool;
        
        private const string UI_ITEM_PREFAB_PATH = "UIInventoryItem";

        private void Awake()
        {
            itemPool = new ObjectPool<UIInventoryItem>(transform);
        }

        private void Start()
        {
            // Subscribe to events
            EventManager.Instance.StartListening<EventData.RequestUIInventoryItem>(OnRequestUIInventoryItem);
            EventManager.Instance.StartListening<EventData.ReturnUIInventoryItem>(OnReturnUIInventoryItem);
            
            // Preload items for the pool
            PreloadItems();
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventManager.Instance?.StopListening<EventData.RequestUIInventoryItem>(OnRequestUIInventoryItem);
            EventManager.Instance?.StopListening<EventData.ReturnUIInventoryItem>(OnReturnUIInventoryItem);
            
            // Dispose the pool
            itemPool.Dispose();
        }

        private async void PreloadItems()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                var item = await itemPool.Get(UI_ITEM_PREFAB_PATH);
                if (item != null)
                {
                    // Return to pool immediately
                    item.gameObject.SetActive(false);
                    itemPool.Return(UI_ITEM_PREFAB_PATH, item);
                }
            }
        }

        private async void OnRequestUIInventoryItem(EventData.RequestUIInventoryItem data)
        {
            try
            {
                // Get an item from the pool
                var item = await itemPool.Get(UI_ITEM_PREFAB_PATH);
                
                if (item != null)
                {
                    data.OnItemCreated?.Invoke(item);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error requesting UI inventory item: {ex.Message}");
            }
        }

        private void OnReturnUIInventoryItem(EventData.ReturnUIInventoryItem data)
        {
            if (data.Item != null)
            {
                // Reset the item
                data.Item.transform.SetParent(transform);
                data.Item.gameObject.SetActive(false);
                
                // Return to pool
                itemPool.Return(UI_ITEM_PREFAB_PATH, data.Item);
            }
        }
    }
}
