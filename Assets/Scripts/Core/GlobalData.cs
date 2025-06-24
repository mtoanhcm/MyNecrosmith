using UnityEngine;

namespace Gameplay { 
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public enum GameplayEventType
    {
        StartGame,
        PauseGame,
        Gameover,
        WinGame,
    }
}
