using Core.UI;
using Observer;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class UIGameover : UIView
    {
        [SerializeField]
        private GameObject gameoverPanel;
        [SerializeField]
        private Button menuBtn;
        [SerializeField]
        private Button restartBtn;

        private void Start()
        {
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.AddListener(() => 
            {
                EventManager.Instance.TriggerEvent(new EventData.OnRequestChangeScene() { SceneType = SceneManage.SceneType.Menu });
            });

            restartBtn.onClick.RemoveAllListeners();
            restartBtn.onClick.AddListener(() => 
            {
                EventManager.Instance.TriggerEvent(new EventData.OnRequestChangeScene() { SceneType = SceneManage.SceneType.Gameplay });
            });
        }
    }
    
}
