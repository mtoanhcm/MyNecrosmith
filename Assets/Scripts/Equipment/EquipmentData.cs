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
        
        public EquipmentID ID => baseData.ID;
        public EquipmentCategoryID Category => baseData.CategoryID;
        public string Name => baseData.EquipmentName;
        public int EffectValue => baseData.EffectValue;
        public float AttackRadius => baseData.AttackRadius;
        public float AttackSpeed => baseData.AttackSpeed;
        public Sprite IconSpr => baseData.Icon;
        public int Width => baseData.Width;
        public int Height => baseData.Height;

        public EquipmentData(EquipmentConfig baseData)
        {
            this.baseData = baseData;
        }
    }
}
