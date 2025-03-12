using System;
using System.Collections.Generic;
using GameUtility;
using Unity.VisualScripting;
using UnityEngine;

namespace Observer {
    public class EventManager : SingletonForScene<EventManager>
    {
        private Dictionary<Type, Delegate> eventDictionary = new Dictionary<Type, Delegate>();

        public void StartListening<T>(Action<T> listener) where T : class
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

        public void StopListening<T>(Action<T> listener) where T : class
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

        public void TriggerEvent<T>(T eventArgs) where T : class
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

        public override void OnDestroy()
        {
            eventDictionary.Clear();
            
            base.OnDestroy();
        }
    }
}
