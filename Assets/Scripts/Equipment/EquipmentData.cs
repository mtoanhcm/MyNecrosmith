using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Config;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class EquipmentData
    {
        [SerializeField]
        protected EquipmentConfig baseConfig;

        public WeaponConfig WeaponConfig => baseConfig as WeaponConfig;
        public virtual string ID => string.Empty;
        public virtual string CategoryID => baseConfig.CategoryID.ToString();
        public virtual string EffectType => string.Empty;
        public virtual string EffectValue => string.Empty;
        public string Name => baseConfig.EquipmentName;
        public Sprite IconSpr => baseConfig.Icon;
        public int LoadPoint => baseConfig.LoadPoint;
        public int Width => baseConfig.Width;
        public int Height => baseConfig.Height;
        public float Damage => WeaponConfig.Damage;
        public float AttackRange => WeaponConfig.AttackRange;
        public float AttackSpeed => WeaponConfig.AttackSpeed;

        public EquipmentData(EquipmentConfig config)
        {
            baseConfig = config;
        }
    }
}
