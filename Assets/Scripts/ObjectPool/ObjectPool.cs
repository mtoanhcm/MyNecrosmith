using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool {
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly List<T> pool;
        private readonly T prefab;
        private readonly Transform parent;
        private readonly int expansionAmount; // N - the number of objects to instantiate when pool is full

        public ObjectPool(T prefab, int initialSize, int expansionAmount = 1, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.expansionAmount = expansionAmount;
            pool = new List<T>();

            AddObjectsToPool(initialSize);
        }

        private void AddObjectsToPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = Object.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                pool.Add(obj);
            }
        }

        public T Get()
        {
            foreach (T obj in pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            // If no inactive object is found, instantiate N objects
            AddObjectsToPool(expansionAmount);
            return Get(); // Try to get an object again after expansion
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
