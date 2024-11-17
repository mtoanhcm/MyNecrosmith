using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class EquipmentData
    {
        [SerializeField]
        private EquipmentConfig baseData;
        
        public string ID => string.Empty;
        public string Name => baseData.EquipmentName;
        public int EffectValue => 0;
        public float AttackRadius => 0;
        public float AttackSpeed => 0;
        public Sprite IconSpr => baseData.Icon;
        public int Width => baseData.Width;
        public int Height => baseData.Height;

        public EquipmentData(EquipmentConfig baseData)
        {
            this.baseData = baseData;
        }
        
        
    }
}
