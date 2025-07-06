using GameUtility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SceneManage
{

    public class SceneManager
    {
        private Dictionary<SceneType, string> scenePaths = new Dictionary<SceneType, string>()
        {
            { SceneType.Menu, "HomeMenuScene" },
            { SceneType.Gameplay, "GameplayScene" },
            { SceneType.Loading, "LoaingScene" }
        };

        public async Task LoadSceneAsync(SceneType sceneType) { 
            var handle = Addressables.LoadSceneAsync(scenePaths[sceneType], UnityEngine.SceneManagement.LoadSceneMode.Single);
            await handle.Task;

            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) {
            }
        }

        public async Task UnloadSceneAsync(SceneType sceneType, Action OnUnloadSceneComplete) {

        }
    }
}
