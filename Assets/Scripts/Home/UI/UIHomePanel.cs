using Core.UI;
using DG.Tweening;
using Observer;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Home.UI
{
    public class UIHomePanel : UIView
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Button startGameBtn;
        [SerializeField]
        private Button exitGameBtn;

        private void Awake()
        {
            startGameBtn.onClick.AddListener(OnStartGameClicked);
            exitGameBtn.onClick.AddListener(OnExitGameClicked);
        }

        private void OnExitGameClicked()
        {
            Application.Quit();
        }

        private void OnStartGameClicked()
        {
            canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                EventManager.Instance.TriggerEvent(new EventData.OnRequestChangeScene() { SceneType = SceneManage.SceneType.Gameplay });
            });
        }
    }
}
