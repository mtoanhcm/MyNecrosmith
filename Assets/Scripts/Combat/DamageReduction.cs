using System.Collections.Generic;
using Config;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Combat
{
    public static class DamageReduction
    {
        private static bool isInit;

        /// <summary>
        /// Key: tuple damage type and armor type
        /// Value: tuple damage addition and reduction
        /// </summary>
        private static Dictionary<(DamageType, ArmorType), (float, float)> damageReductionDic;
        
        public static void Init()
        {
            if (isInit)
            {
                return;
            }
            
            damageReductionDic = new Dictionary<(DamageType, ArmorType), (float, float)>();

            Addressables.LoadAssetAsync<DamageReductionConfig>("Config/Combat/DamageReductionConfig.asset").Completed +=
                handle =>
                {
                    var damageEffectInfos = handle.Result.DamageEffectInfos;
                    for(var i =0; i < damageEffectInfos.Length; i++)
                    {
                        var info = damageEffectInfos[i];
                        for (var j = 0; j < info.EffectArmors.Length; j++)
                        {
                            var effectByArmor = info.EffectArmors[j];

                            damageReductionDic[(info.DamageType, effectByArmor.ArmorType)] =
                                (effectByArmor.Addition, effectByArmor.Reduction);
                        }
                        
                    }
                    
                    isInit = true;
                };
        }
        
        public static float GetDamageBonus(DamageType damageType, ArmorType armorType)
        {
            float bonus = 0;
            if (!isInit)
            {
                Debug.LogError("The damage reduction dictionary is not initialized.");
                return bonus;
            }

            if (damageReductionDic.TryGetValue((damageType, armorType), out var damageEffect))
            {
                bonus = damageEffect.Item1 - damageEffect.Item2;
            }
            
            return bonus;
        }
    }   
}
