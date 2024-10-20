using System.Collections;
using System.Collections.Generic;
using Config;
using UnityEngine;

namespace Equipment
{
    public class EquipmentData
    {
        private readonly EquipmentConfig baseData;
        
        public ItemID ID => baseData.ID;
        public string Name => baseData.EquipmentName;
        public int EffectValue => baseData.EffectValue;
        public Sprite IconSpr => baseData.Icon;
        public int Width => baseData.Width;
        public int Height => baseData.Height;

        public EquipmentData(EquipmentConfig baseData)
        {
            this.baseData = baseData;
        }
    }   
}
