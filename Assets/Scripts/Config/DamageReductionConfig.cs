using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Config
{
    [CreateAssetMenu(fileName = "DamageReductionConfig", menuName = "baseConfig/DamageReductionConfig")]
    public class DamageReductionConfig : ScriptableObject
    {
        [Serializable]
        public struct DamageEffectData
        {
            public DamageType DamageType;
            public DamageEffectArmor[] EffectArmors;
        }

        [Serializable]
        public struct DamageEffectArmor
        {
            public ArmorType ArmorType;
            public float Addition;
            public float Reduction;
        }

        public DamageEffectData[] DamageEffectInfos => damageEffectInfos;

        [SerializeField]
        private DamageEffectData[] damageEffectInfos;

    }   
}
