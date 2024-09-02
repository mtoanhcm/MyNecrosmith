using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Observer {
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance
        {
            get {
                if (instance == null) {

                    instance = FindObjectOfType<EventManager>();

                    if (instance == null)
                    {
                        GameObject obj = new ("EventManager");
                        instance = obj.AddComponent<EventManager>();
                        DontDestroyOnLoad(obj); // Optional: Persist across scenes
                    }
                }

                return instance;
            }
        }

        private static EventManager instance;
        private Dictionary<Type, Delegate> eventDictionary = new Dictionary<Type, Delegate>();

        private void OnDestroy()
        {
            instance = null;
        }

        public void StartListening<T>(Action<T> listener) where T : struct
        {
            var eventType = typeof(T);
            if (eventDictionary.TryGetValue(eventType, out var existingDelegate))
            {
                eventDictionary[eventType] = Delegate.Combine(existingDelegate, listener);
            }
            else
            {
                eventDictionary[eventType] = listener;
            }
        }

        public void StopListening<T>(Action<T> listener) where T : struct
        {
            var eventType = typeof(T);
            if (eventDictionary.TryGetValue(eventType, out var existingDelegate))
            {
                var currentDelegate = Delegate.Remove(existingDelegate, listener);

                if (currentDelegate == null)
                {
                    eventDictionary.Remove(eventType);
                }
                else
                {
                    eventDictionary[eventType] = currentDelegate;
                }
            }
        }

        public void TriggerEvent<T>(T eventArgs) where T : struct
        {
            var eventType = typeof(T);
            if (eventDictionary.TryGetValue(eventType, out var existingDelegate))
            {
                if (existingDelegate is Action<T> callback)
                {
                    callback.Invoke(eventArgs);
                }
            }
        }
    }
}
