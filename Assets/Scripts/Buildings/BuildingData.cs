using System;
using Config;

namespace Building {

    public enum BuildingID { 
        Treasure,
        EnemyFortress,
        MainBase
    }
    
    [Serializable]
    public class BuildingData
    {
        protected readonly BuildingConfig baseConfig;
        public BuildingID ID => baseConfig.ID;
        public int CurrentHP;
        public int MaxHP => baseConfig.HP;
        public int AreaIndex { get; private set; }
        public int Level { get; private set; }

        public BuildingData(BuildingConfig baseConfig, int areaIndex, int level)
        {
            this.baseConfig = baseConfig;
            CurrentHP = baseConfig.HP;
            Level = level;
            AreaIndex = areaIndex;
        }

        public virtual void LevelUp()
        {
            Level++;
        }
    }
}
