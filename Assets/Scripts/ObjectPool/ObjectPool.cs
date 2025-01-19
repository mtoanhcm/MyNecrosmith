using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace Pool
{
    /// <summary>
    /// High-performance object pool for Unity components.
    /// Supports automatic pool expansion and object recycling.
    /// </summary>
    public class ObjectPool<T> : IDisposable where T : Component
    {
        private readonly struct PoolKey : System.IEquatable<PoolKey>
        {
            private readonly int hashCode;
            public readonly string Id;

            public PoolKey(string id)
            {
                Id = id;
                hashCode = id.GetHashCode();
            }

            public bool Equals(PoolKey other) => Id == other.Id;
            public override bool Equals(object obj) => obj is PoolKey key && Equals(key);
            public override int GetHashCode() => hashCode;
        }

        private class PoolData
        {
            public readonly Stack<T> AvailableItems;
            public readonly HashSet<T> UsedItems;
            public GameObject SourcePrefab;
            public AsyncOperationHandle<GameObject> LoadHandle;
            public bool IsLoading;

            public PoolData(int initialCapacity)
            {
                AvailableItems = new Stack<T>(initialCapacity);
                UsedItems = new HashSet<T>();
            }
        }

        private readonly Dictionary<PoolKey, PoolData> pools;
        private readonly int initialSize;
        private readonly int maxSize;
        private readonly Transform root;
        private bool isDisposed;

        public ObjectPool(Transform parent = null, int initialSize = 10, int maxSize = 100)
        {
            this.pools = new Dictionary<PoolKey, PoolData>();
            this.initialSize = initialSize;
            this.maxSize = maxSize;
            this.root = parent;
        }

        public bool TryGet(string itemId, out T instance)
        {
            ThrowIfDisposed();
            var key = new PoolKey(itemId);
            instance = null;

            if (!pools.TryGetValue(key, out var data))
                return false;

            if (data.AvailableItems.Count == 0)
                return false;

            instance = data.AvailableItems.Pop();
            data.UsedItems.Add(instance);
            instance.gameObject.SetActive(true);
            return true;
        }

        public async Task<T> Get(string itemId)
        {
            ThrowIfDisposed();
            var key = new PoolKey(itemId);

            // Get or create pool for this prefab
            if (!pools.TryGetValue(key, out var data))
            {
                data = await InitializePool(key, itemId);
                pools[key] = data;
            }

            // Fast path: get from available items
            if (data.AvailableItems.Count > 0)
            {
                var instance = data.AvailableItems.Pop();
                data.UsedItems.Add(instance);
                instance.gameObject.SetActive(true);
                return instance;
            }

            // Check if pool needs and can expand
            int currentTotal = data.UsedItems.Count + data.AvailableItems.Count;
            if (currentTotal >= maxSize)
            {
                Debug.LogWarning($"Pool for {itemId} has reached maximum size of {maxSize}");
                return await WaitForAvailableInstance(data);
            }

            // Calculate how many new instances we can create
            int remainingSpace = maxSize - currentTotal;
            int expandAmount = Mathf.Min(initialSize, remainingSpace);

            // Create new instances
            for (int i = 0; i < expandAmount - 1; i++)
            {
                var instance = CreateInstance(data.SourcePrefab);
                instance.gameObject.SetActive(false);
                data.AvailableItems.Push(instance);
            }

            // Create and return the last instance
            var newInstance = CreateInstance(data.SourcePrefab);
            data.UsedItems.Add(newInstance);
            return newInstance;
        }

        public void Return(string itemId, T instance)
        {
            ThrowIfDisposed();
            var key = new PoolKey(itemId);

            if (!pools.TryGetValue(key, out var data))
            {
                Debug.LogWarning($"Attempted to return instance to non-existent pool: {itemId}");
                return;
            }

            if (!data.UsedItems.Remove(instance))
            {
                Debug.LogWarning($"Instance {instance} was not acquired from pool {itemId}");
                return;
            }

            instance.gameObject.SetActive(false);
            data.AvailableItems.Push(instance);
        }

        private async Task<PoolData> InitializePool(PoolKey key, string itemId)
        {
            var data = new PoolData(initialSize);

            try
            {
                data.IsLoading = true;
                data.LoadHandle = Addressables.LoadAssetAsync<GameObject>(itemId);
                await data.LoadHandle.Task;

                if (data.LoadHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    throw new System.Exception($"Failed to load asset: {itemId}");
                }

                data.SourcePrefab = data.LoadHandle.Result;

                if (data.SourcePrefab.GetComponent<T>() == null)
                {
                    throw new System.Exception($"Prefab {itemId} missing required component: {typeof(T)}");
                }

                // Create initial pool size
                for (int i = 0; i < initialSize; i++)
                {
                    var instance = CreateInstance(data.SourcePrefab);
                    instance.gameObject.SetActive(false);
                    data.AvailableItems.Push(instance);
                }

                return data;
            }
            finally
            {
                data.IsLoading = false;
            }
        }

        private T CreateInstance(GameObject prefab)
        {
            var instance = UnityEngine.Object.Instantiate(prefab, root);
            return instance.GetComponent<T>();
        }

        private async Task<T> WaitForAvailableInstance(PoolData data)
        {
            while (data.AvailableItems.Count == 0)
            {
                await Task.Yield();
            }

            var instance = data.AvailableItems.Pop();
            data.UsedItems.Add(instance);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Dispose()
        {
            if (isDisposed) return;

            try
            {
                foreach (var pool in pools.Values)
                {
                    try
                    {
                        if (pool.LoadHandle.IsValid())
                        {
                            Addressables.Release(pool.LoadHandle);
                        }

                        foreach (var instance in pool.UsedItems)
                        {
                            if (instance != null)
                            {
                                UnityEngine.Object.Destroy(instance.gameObject);
                            }
                        }

                        while (pool.AvailableItems.Count > 0)
                        {
                            var instance = pool.AvailableItems.Pop();
                            if (instance != null)
                            {
                                UnityEngine.Object.Destroy(instance.gameObject);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error during pool cleanup: {e}");
                    }
                }

                pools.Clear();
            }
            finally
            {
                isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(ObjectPool<T>));
            }
        }
    }
}