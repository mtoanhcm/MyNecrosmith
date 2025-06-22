using Observer;
using SceneManage;
using System;
using UnityEngine;

namespace Home {
    public class HomeManager : MonoBehaviour
    {
        private SceneManager sceneManager;

        private void Awake()
        {
            // Initialize the SceneManager instance
            sceneManager = new SceneManager();
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnRequestChangeScene>(OnRequestChangeScene);
        }

        private void OnRequestChangeScene(EventData.OnRequestChangeScene data)
        {
            sceneManager.LoadSceneAsync(data.SceneType);
        }
    }
}
