using System.Collections.Generic;
using GameUtility;
using Observer;
using UnityEngine;

namespace Spawner
{
    public abstract class ObjectSpawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected GameObjectPool<T> objectPool;
        protected Dictionary<string, T> prefabDictionary = new Dictionary<string, T>();

        protected virtual void Start()
        {
            objectPool = new GameObjectPool<T>(CreateObjectForPool, OnGetObjectFromPool, OnReturnObjectToPool, transform);
            prefabDictionary = new Dictionary<string, T>();
        }

        protected virtual T CreateObjectForPool(string typeID)
        {
            if (!prefabDictionary.TryGetValue(typeID, out var prefab))
            {
                Debug.LogError($"{typeof(T).Name} {typeID} resources cannot prepare");
                return null;
            }

            return Instantiate(prefab, transform);
        }

        protected virtual void OnReturnObjectToPool(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected virtual void OnGetObjectFromPool(T obj)
        {
            obj.gameObject.SetActive(true);
        }
    }   
}
