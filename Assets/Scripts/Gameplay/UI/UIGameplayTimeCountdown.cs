using Core.UI;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{

    public class UIGameplayTimeCountdown : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timeTxt;
        [SerializeField]
        private GameMode gameMode;

        private void Update()
        {
            timeTxt.text = $"{gameMode.GameModePlayTime / 60:00}:{gameMode.GameModePlayTime % 60:00}";
        }

    }
}
