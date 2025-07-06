using SceneManage;
using UnityEngine;

namespace Bootstrap {
    public class LandingManager : MonoBehaviour
    {
        private SceneManager sceneManager;

        private void Awake()
        {
            sceneManager = new SceneManager();
        }

        private void Start()
        {
            sceneManager.LoadSceneAsync(SceneType.Menu);
        }
    }
}
