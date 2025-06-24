using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "EquipmentDropRateConfig", menuName = "BaseConfig/Equipment/EquipmentDropRateConfig")]
    public class EquipmentDropRateConfig : ScriptableObject
    {
        [Serializable]
        public class DropRateInfo
        {
            public EquipmentConfig equipment;
            public float DropRate;
        }

        [SerializeField]
        private float emptyRate;
        [SerializeField]
        private List<DropRateInfo> dropRates;

        private float totalRate;

        public void Init()
        {
            foreach (var rate in dropRates)
            {
                totalRate += rate.DropRate;
            }

            totalRate += emptyRate;
        }

        public bool GetRandomEquipmentID(out EquipmentConfig equipmentConfig)
        {
            float randomValue = UnityEngine.Random.Range(0f, totalRate);
            float cumulativeRate = 0f;

            equipmentConfig = null;

            foreach (var rate in dropRates)
            {
                cumulativeRate += rate.DropRate;
                if (randomValue <= cumulativeRate)
                {
                    equipmentConfig = rate.equipment;
                    return true;
                }
            }

            return false;
        }
    }
}
