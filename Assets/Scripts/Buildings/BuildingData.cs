using System;
using Config;
using UnityEngine;

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
        public ArmorType ArmorType => baseConfig.ArmorType;
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

        public void TakeDamage(int damage, Action onDestroyCallback)
        {
            CurrentHP -= damage;
            
            CurrentHP = Mathf.Clamp(CurrentHP, 0, baseConfig.HP);
            if (CurrentHP <= 0)
            {
                onDestroyCallback?.Invoke();
            }
        }

        public virtual void LevelUp()
        {
            Level++;
        }
    }
}
