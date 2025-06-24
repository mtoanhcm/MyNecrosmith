using Building;
using Observer;
using UnityEngine;

namespace Gameplay {
    public class SurvivalGameMode : GameMode
    {

        [SerializeField]
        private float gameTimeDuration;
        [SerializeField]
        private HighTower mainTower;

        protected override void InitGameMode()
        {
            gameModePlayTime = gameTimeDuration;
        }

        protected override void OnUpdate()
        {
            gameModePlayTime--;
            if(gameModePlayTime < 0)
            {
                gameModePlayTime = 0;
            }
        }

        protected override bool LoseCondition()
        {
            return mainTower != null && mainTower.Health != null && !mainTower.Health.IsAlive;
        }

        protected override bool WinCondition()
        {
            return gameModePlayTime <= 0;
        }
    }
}
