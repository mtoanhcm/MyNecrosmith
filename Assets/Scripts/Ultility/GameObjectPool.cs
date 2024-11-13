using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameUtility
{
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        private Dictionary<string, Queue<T>> pool;

        private event Func<string, T> onCreateNewObj;
        private event Action<T> onGetObject;
        private event Action<T> onReturnObj;
        
        private readonly int poolAddAmount;
        private readonly Transform parent;

        public GameObjectPool(Func<string, T> onCreateObj, Action<T> onGetObjFromPool, Action<T> onReturnObjToPool, Transform parent = null, int poolAddAmount = 5)
        {
            Assert.IsNotNull(onCreateObj, "Create object action cannot be null");
            
            this.parent = parent;
            this.poolAddAmount = poolAddAmount;
            
            pool = new Dictionary<string, Queue<T>>();
            
            onCreateNewObj = onCreateObj;
            onGetObject = onGetObjFromPool;
            onReturnObj = onReturnObjToPool;
        }

        public T GetObject(string type)
        {
            if (!pool.ContainsKey(type) || pool[type].Count == 0)
            {
                CreateObjTypeToPool(type);
            }
            
            var obj = pool[type].Dequeue();
            onGetObject?.Invoke(obj);
            
            return obj;
        }

        public void ReturnToPool(string type, T obj)
        {
            onReturnObj?.Invoke(obj);
            
            if (pool.ContainsKey(type))
            {
                pool[type].Enqueue(obj);
                return;
            }
            
            CreateObjTypeToPool(type);
        }

        private void CreateObjTypeToPool(string type)
        {
            if (!pool.ContainsKey(type))
            {
                pool.Add(type, new Queue<T>());
            }

            for (var i = 0; i < poolAddAmount; i++)
            {
                var obj = onCreateNewObj(type);
                Assert.IsNotNull(obj, $"Cannot create pool object {type} because the prefab is null");
                
                obj.SetActive(false);
                pool[type].Enqueue(obj);
            }
        }
    }   
}
