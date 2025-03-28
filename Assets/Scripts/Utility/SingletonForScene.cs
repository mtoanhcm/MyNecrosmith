using System;
using UnityEngine;

namespace GameUtility
{
    public class SingletonForScene<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Static instance, private to prevent direct access
        private static T instance;

        // Lock to ensure thread-safety
        private static readonly object @lock = new object();

        // Event triggered when instance is destroyed so other objects can unsubscribe
        public static event Action OnInstanceDestroyed;

        // Property to access the instance
        public static T Instance
        {
            get
            {
                // Check if we're quitting the application
                if (applicationIsQuitting)
                {
                    Debug.LogWarning(
                        $"[Singleton] Instance '{typeof(T)}' has been destroyed during application quit. Won't create again.");
                    return null;
                }

                // Double-check locking pattern
                lock (@lock)
                {
                    if (instance == null)
                    {
                        // Find all instances in the scene
                        T[] instances = FindObjectsOfType<T>();

                        // If there's more than one instance
                        if (instances.Length > 1)
                        {
                            Debug.LogError(
                                $"[Singleton] More than one instance of Singleton class '{typeof(T)}' found. This is an error!");
                            return instances[0];
                        }
                        // If there's exactly one instance, use it
                        else if (instances.Length == 1)
                        {
                            instance = instances[0];
                            Debug.Log($"[Singleton] Using existing instance of '{typeof(T)}'.");
                        }
                        // If no instance exists, create a new one
                        else
                        {
                            GameObject singleton = new GameObject();
                            instance = singleton.AddComponent<T>();
                            singleton.name = $"(Singleton) {typeof(T)}";

                            // Don't use DontDestroyOnLoad to allow the instance to be destroyed when changing scenes
                            Debug.Log($"[Singleton] Created new instance of '{typeof(T)}'.");
                        }
                    }

                    return instance;
                }
            }
        }

        // Variable to check if the application is quitting
        private static bool applicationIsQuitting = false;

        /// <summary>
        /// When MonoBehaviour is destroyed, mark that the application is quitting and notify other objects
        /// </summary>
        public virtual void OnDestroy()
        {
            // If the instance being destroyed is the current instance
            if (instance == this)
            {
                // Call event to notify subscribed objects
                if (OnInstanceDestroyed != null)
                {
                    OnInstanceDestroyed.Invoke();
                
                    // Clear all delegates to prevent memory leaks
                    OnInstanceDestroyed = null;
                }
            
                // Reset instance
                instance = null;
            }
        }

        /// <summary>
        /// Method to actively release the instance and its references
        /// </summary>
        public static void ReleaseInstance()
        {
            if (instance != null)
            {
                // Notify subscribed objects
                OnInstanceDestroyed?.Invoke();

                // Destroy GameObject
                Destroy(instance.gameObject);

                // Reset instance
                instance = null;

                Debug.Log($"[Singleton] Instance '{typeof(T)}' has been actively released.");
            }
        }

        /// <summary>
        /// Method to check if the instance has been initialized without creating a new instance
        /// </summary>
        public static bool HasInstance
        {
            get { return instance != null && !applicationIsQuitting; }
        }

        /// <summary>
        /// Method called when the scene is loaded
        /// </summary>
        protected virtual void Awake()
        {
            // Set applicationIsQuitting to false if singleton is created in a new scene
            applicationIsQuitting = false;
        }
    }
}
