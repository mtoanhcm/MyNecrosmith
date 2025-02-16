using System;
using Config;

namespace Building {

    public enum BuildingType { 
        Treasure,
        EnemyBase,
        MainBase
    }
    
    [Serializable]
    public class BuildingData
    {
        protected readonly BuildingConfig baseConfig;
        public int CurrentHP;
        public int MaxHP => baseConfig.HP;
        public int Level;

        public BuildingData(BuildingConfig baseConfig)
        {
            this.baseConfig = baseConfig;
            CurrentHP = baseConfig.HP;
            Level = 1;
        }

        public virtual void LevelUp()
        {
            Level++;
        }
    }
}
