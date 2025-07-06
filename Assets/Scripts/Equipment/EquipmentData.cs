using System;
using Config;
using Gameplay;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class EquipmentData
    {
        [SerializeField]
        protected EquipmentConfig baseConfig;

        public int Level => level;
        public EquipmentID EquipmentID => baseConfig.EquipmentID;
        public EquipmentCategoryID CategoryID => baseConfig.CategoryID;
        public EquipmentGroup Group => baseConfig.Group;
        public Rarity Rarity => baseConfig.Rarity;
        public virtual string EffectType => string.Empty;
        public virtual string EffectValue => string.Empty;
        public string Name => baseConfig.EquipmentName;
        public Sprite IconSpr => baseConfig.Icon;
        public int LoadPoint => baseConfig.LoadPoint;
        public int Width => baseConfig.Width;
        public int Height => baseConfig.Height;

        protected int level;
        
        public EquipmentData(EquipmentConfig config)
        {
            baseConfig = config;
            level = 1;
        }
    }
}
