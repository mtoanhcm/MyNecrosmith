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
            public int MaxSize; // Dynamic max size that can grow

            public PoolData(int initialCapacity, int maxSize)
            {
                AvailableItems = new Stack<T>(initialCapacity);
                UsedItems = new HashSet<T>();
                MaxSize = maxSize;
            }
        }

        private readonly Dictionary<PoolKey, PoolData> pools;
        private readonly int initialSize;
        private readonly int defaultMaxSize;
        private readonly Transform root;
        private bool isDisposed;
        
        // Number of instances to create on each expansion
        private readonly int expansionSize = 5;
        // Amount to increase max size when expansion is needed
        private readonly int maxSizeIncrement = 10;

        /// <summary>
        /// Creates a new object pool.
        /// </summary>
        /// <param name="parent">Parent transform for instantiated objects</param>
        /// <param name="initialSize">Initial number of objects to create in each pool</param>
        /// <param name="maxSize">Initial maximum size for each pool</param>
        public ObjectPool(Transform parent = null, int initialSize = 10, int maxSize = 100)
        {
            this.pools = new Dictionary<PoolKey, PoolData>();
            this.initialSize = initialSize;
            this.defaultMaxSize = maxSize;
            this.root = parent;
        }

        /// <summary>
        /// Attempts to get an instance from the pool without creating new instances.
        /// </summary>
        /// <param name="itemId">Addressable ID of the prefab</param>
        /// <param name="instance">Output instance if available</param>
        /// <returns>True if an instance was retrieved, false otherwise</returns>
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

        /// <summary>
        /// Gets an instance from the pool, creating a new one if necessary.
        /// Will expand the pool if needed.
        /// </summary>
        /// <param name="itemId">Addressable ID of the prefab</param>
        /// <returns>An instance of the requested type</returns>
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
            
            // If pool is at capacity, expand it
            if (currentTotal >= data.MaxSize)
            {
                // Increase the maximum size of the pool
                data.MaxSize += maxSizeIncrement;
                
                // Expand the pool and get an instance
                return await ExpandPoolAndGetInstance(data, itemId);
            }

            // Calculate how many new instances we can create
            int remainingSpace = data.MaxSize - currentTotal;
            int expandAmount = Mathf.Min(expansionSize, remainingSpace);

            List<T> newInstances = new List<T>(expandAmount);
            
            // Create new instances
            for (int i = 0; i < expandAmount - 1; i++)
            {
                try
                {
                    var instance = CreateInstance(data.SourcePrefab);
                    if (instance != null)
                    {
                        instance.gameObject.SetActive(false);
                        newInstances.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error creating instance during expansion: {ex.Message}");
                }
            }
            
            // Push all successfully created instances to the pool
            foreach (var instance in newInstances)
            {
                data.AvailableItems.Push(instance);
            }

            // Create and return the last instance
            try
            {
                var newInstance = CreateInstance(data.SourcePrefab);
                if (newInstance != null)
                {
                    data.UsedItems.Add(newInstance);
                    return newInstance;
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create new instance of {itemId}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating final instance: {ex.Message}");
                
                // If we have any available items now (from successful expansion), use one of those
                if (data.AvailableItems.Count > 0)
                {
                    var instance = data.AvailableItems.Pop();
                    data.UsedItems.Add(instance);
                    instance.gameObject.SetActive(true);
                    return instance;
                }
                
                // Otherwise wait for an instance
                return await WaitForAvailableInstance(data);
            }
        }

        /// <summary>
        /// Expands the pool beyond its current max size and returns a new instance.
        /// Creates multiple instances at once to improve efficiency.
        /// </summary>
        private async Task<T> ExpandPoolAndGetInstance(PoolData data, string itemId)
        {
            // Create multiple new instances at once
            List<T> newInstances = new List<T>(expansionSize);
            
            for (int i = 0; i < expansionSize; i++)
            {
                try
                {
                    var instance = CreateInstance(data.SourcePrefab);
                    if (instance != null)
                    {
                        newInstances.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error creating instance during forced expansion: {ex.Message}");
                }
            }
            
            if (newInstances.Count == 0)
            {
                Debug.LogError($"Failed to create any instances during forced expansion for {itemId}");
                return await WaitForAvailableInstance(data);
            }
            
            // Take one instance to return immediately
            T instanceToReturn = newInstances[0];
            newInstances.RemoveAt(0);
            
            // Add the remaining instances to the pool
            foreach (var instance in newInstances)
            {
                instance.gameObject.SetActive(false);
                data.AvailableItems.Push(instance);
            }
            
            // Mark the instance as being used
            data.UsedItems.Add(instanceToReturn);
            instanceToReturn.gameObject.SetActive(true);
            
            return instanceToReturn;
        }

        /// <summary>
        /// Returns an instance to the pool, making it available for reuse.
        /// </summary>
        /// <param name="itemId">Addressable ID of the prefab</param>
        /// <param name="instance">Instance to return to the pool</param>
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

        /// <summary>
        /// Initializes a new pool for the specified prefab.
        /// Creates the initial set of instances.
        /// </summary>
        private async Task<PoolData> InitializePool(PoolKey key, string itemId)
        {
            var data = new PoolData(initialSize, defaultMaxSize);

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

                if (data.SourcePrefab == null)
                {
                    throw new System.Exception($"Loaded prefab is null for {itemId}");
                }

                if (data.SourcePrefab.GetComponent<T>() == null)
                {
                    throw new System.Exception($"Prefab {itemId} missing required component: {typeof(T)}");
                }

                // Create initial pool size
                List<T> instances = new List<T>(initialSize);
                for (int i = 0; i < initialSize; i++)
                {
                    try
                    {
                        var instance = CreateInstance(data.SourcePrefab);
                        if (instance != null)
                        {
                            instance.gameObject.SetActive(false);
                            instances.Add(instance);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error creating instance during initialization: {ex.Message}");
                    }
                }
                
                // Push all successfully created instances to the pool
                foreach (var instance in instances)
                {
                    data.AvailableItems.Push(instance);
                }

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during pool initialization: {ex.Message}");
                throw;
            }
            finally
            {
                data.IsLoading = false;
            }
        }

        /// <summary>
        /// Creates a new instance of the specified prefab.
        /// </summary>
        private T CreateInstance(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.LogError("Cannot create instance from null prefab");
                return null;
            }
            
            try
            {
                var gameObject = UnityEngine.Object.Instantiate(prefab, root);
                var component = gameObject.GetComponent<T>();
                
                if (component == null)
                {
                    Debug.LogError($"Instantiated prefab does not have component {typeof(T).Name}");
                    UnityEngine.Object.Destroy(gameObject);
                    return null;
                }
                
                return component;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during prefab instantiation: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Waits for an instance to become available in the pool.
        /// Will periodically attempt to expand the pool while waiting.
        /// </summary>
        private async Task<T> WaitForAvailableInstance(PoolData data)
        {
            int timeoutSeconds = 10; // Reduced timeout
            float startTime = Time.realtimeSinceStartup;
            bool timeoutWarned = false;
            int attemptCount = 0;
            
            while (data.AvailableItems.Count == 0)
            {
                // Increment attempt counter
                attemptCount++;
                
                // Check for timeout
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                if (elapsedTime > 3f && !timeoutWarned)
                {
                    Debug.LogWarning($"Waiting for more than 3 seconds for an available instance. Used items: {data.UsedItems.Count}");
                    timeoutWarned = true;
                }
                
                // Every 20 attempts (about 0.3 seconds), try expanding the pool again
                if (attemptCount % 20 == 0)
                {
                    // Increase max size again
                    data.MaxSize += maxSizeIncrement;
                    
                    // Create a few more instances
                    for (int i = 0; i < 2; i++) // Only create 2 instances each time to avoid lag
                    {
                        try
                        {
                            var newInstance = CreateInstance(data.SourcePrefab);
                            if (newInstance != null)
                            {
                                newInstance.gameObject.SetActive(false);
                                data.AvailableItems.Push(newInstance);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Failed to create emergency instance during wait: {ex.Message}");
                        }
                    }
                    
                    // If we successfully created any instances, use one immediately
                    if (data.AvailableItems.Count > 0)
                    {
                        break;
                    }
                }
                
                // If we've waited too long, create a final emergency instance
                if (elapsedTime > timeoutSeconds)
                {
                    Debug.LogError($"Timeout waiting for available instance after {timeoutSeconds} seconds");
                    
                    // Final attempt to create an emergency instance
                    try 
                    {
                        var emergencyInstance = CreateInstance(data.SourcePrefab);
                        if (emergencyInstance != null)
                        {
                            data.UsedItems.Add(emergencyInstance);
                            emergencyInstance.gameObject.SetActive(true);
                            return emergencyInstance;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to create final emergency instance: {ex.Message}");
                    }
                    
                    throw new TimeoutException($"Timeout waiting for available instance");
                }
                
                await Task.Yield();
            }

            var instance = data.AvailableItems.Pop();
            data.UsedItems.Add(instance);
            instance.gameObject.SetActive(true);
            return instance;
        }

        /// <summary>
        /// Disposes the object pool, releasing all instances and addressable references.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;

            try
            {
                foreach (var poolEntry in pools)
                {
                    var key = poolEntry.Key;
                    var pool = poolEntry.Value;
                    
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

        /// <summary>
        /// Checks if the pool has been disposed and throws an exception if it has.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(ObjectPool<T>));
            }
        }
    }
}