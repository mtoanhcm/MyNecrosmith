using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameUtility
{
    public class DynamicScrollView : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField] private GameObject itemPrefab;

        [Header("Settings")] [SerializeField] private float itemHeight;
        [SerializeField] private int poolSize = 10;

        private Queue<IItemUI> poolQueue = new Queue<IItemUI>();
        private List<DataItem> allDataItems = new List<DataItem>();
        private Dictionary<int, IItemUI> visibleItems = new Dictionary<int, IItemUI>();

        private int totalItems;
        private int maxVisibleItems;
        private Action<IItemUI, DataItem> onSetupItemCallback;

        private void Start()
        {
            InitializePool();
            maxVisibleItems = Mathf.CeilToInt(scrollRect.viewport.rect.height / itemHeight);
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            UpdateVisibleItems();
        }

        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject itemObject = Instantiate(itemPrefab, scrollRect.content);
                IItemUI item = itemObject.GetComponent<IItemUI>();
                itemObject.SetActive(false);
                poolQueue.Enqueue(item);
            }
        }

        public void SetData(List<DataItem> dataItems, Action<IItemUI, DataItem> onSetupItem)
        {
            allDataItems = dataItems;
            totalItems = allDataItems.Count;
            onSetupItemCallback = onSetupItem;
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, totalItems * itemHeight);
            UpdateVisibleItems();
        }

        private void OnScrollValueChanged(Vector2 scrollPosition)
        {
            UpdateVisibleItems();
        }

        private void UpdateVisibleItems()
        {
            int startIndex = Mathf.Max(0,
                Mathf.FloorToInt((1 - scrollRect.verticalNormalizedPosition) * totalItems) - maxVisibleItems);
            int endIndex = Mathf.Min(totalItems, startIndex + maxVisibleItems * 2);

            HashSet<int> newVisibleIndices = new HashSet<int>();
            for (int i = startIndex; i < endIndex; i++)
            {
                newVisibleIndices.Add(i);
            }

            // Deactivate items that are no longer visible
            foreach (var index in new List<int>(visibleItems.Keys))
            {
                if (!newVisibleIndices.Contains(index))
                {
                    ReturnToPool(visibleItems[index]);
                    visibleItems.Remove(index);
                }
            }

            // Activate new items
            foreach (int i in newVisibleIndices)
            {
                if (!visibleItems.ContainsKey(i))
                {
                    IItemUI newItem = GetPooledObject();
                    newItem.GameObject.transform.SetParent(scrollRect.content, false);
                    newItem.GameObject.transform.localPosition = new Vector2(0, -i * itemHeight);
                    onSetupItemCallback?.Invoke(newItem, allDataItems[i]);
                    visibleItems[i] = newItem;
                }
            }
        }

        private IItemUI GetPooledObject()
        {
            if (poolQueue.Count > 0)
            {
                IItemUI pooledObject = poolQueue.Dequeue();
                pooledObject.GameObject.SetActive(true);
                return pooledObject;
            }

            GameObject newItemObject = Instantiate(itemPrefab, scrollRect.content);
            IItemUI newItem = newItemObject.GetComponent<IItemUI>();
            newItemObject.SetActive(true);
            return newItem;
        }

        private void ReturnToPool(IItemUI item)
        {
            item.GameObject.SetActive(false);
            poolQueue.Enqueue(item);
        }
    }

    [System.Serializable]
    public class DataItem
    {
        public string itemName;
        public string itemDescription;
    }

    public interface IItemUI
    {
        GameObject GameObject { get; }
        void Setup(DataItem data);
    }
}
