using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUtility
{
    public static class AddressableUtility
    {
        /// <summary>
        /// Loads an asset asynchronously using an AssetReference.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="reference">The AssetReference to load the asset from.</param>
        /// <returns>The loaded asset.</returns>
        public static async Task<T> LoadAssetAsync<T>(AssetReference reference) where T : UnityEngine.Object
        {
            if (reference == null)
            {
                Debug.LogError("AssetReference is null.");
                return null;
            }

            var handle = reference.LoadAssetAsync<T>();

            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load Addressable asset from AssetReference: {reference.RuntimeKey}");
                return null;
            }
        }

        /// <summary>
        /// Instantiates a prefab asynchronously using an AssetReference.
        /// </summary>
        /// <param name="reference">The AssetReference to instantiate the prefab from.</param>
        /// <param name="parent">Optional parent for the instantiated prefab.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static async Task<GameObject> InstantiateAsync(AssetReference reference, Transform parent = null)
        {
            if (reference == null)
            {
                Debug.LogError("AssetReference is null.");
                return null;
            }

            var handle = reference.InstantiateAsync(parent);

            try
            {
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return handle.Result;
                }
                else
                {
                    Debug.LogError($"Failed to instantiate prefab from AssetReference: {reference.RuntimeKey}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception occurred during instantiation: {e}");
                return null;
            }
        }

        /// <summary>
        /// Loads an asset asynchronously using an AssetReference.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="url">The URL to load the asset from.</param>
        /// <returns>The loaded asset.</returns>
        public static async Task<T> LoadAssetAsync<T>(string url) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("URL is null or empty.");
                return null;
            }

            var handle = Addressables.LoadAssetAsync<T>(url);

            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load Addressable asset from url: {url}");
                return null;
            }
        }
        
        /// <summary>
        /// Instantiates a prefab asynchronously from a URL.
        /// </summary>
        /// <param name="url">The URL of the prefab to instantiate.</param>
        /// <param name="parent">Optional parent for the instantiated prefab.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static async Task<GameObject> InstantiateAsync(string url, Transform parent = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("URL is null or empty.");
                return null;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(url);

            try
            {
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject instance = UnityEngine.Object.Instantiate(handle.Result, parent);
                    return instance;
                }
                else
                {
                    Debug.LogError($"Failed to load prefab from URL: {url}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception occurred during instantiation from URL: {e}");
                return null;
            }
        }

        /// <summary>
        /// Releases an AssetReference loaded asset.
        /// </summary>
        /// <param name="reference">The AssetReference to release.</param>
        public static void ReleaseAsset(AssetReference reference)
        {
            if (reference == null)
            {
                Debug.LogError("AssetReference is null.");
                return;
            }

            reference.ReleaseAsset();
        }

        /// <summary>
        /// Releases an instantiated prefab.
        /// </summary>
        /// <param name="instance">The instantiated GameObject to release.</param>
        public static void ReleaseInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
        }
    }
}
