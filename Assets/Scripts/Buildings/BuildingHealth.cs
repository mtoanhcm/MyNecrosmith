using Combat;
using InterfaceComp;
using UnityEngine;
using System;

namespace Building
{
    public class BuildingHealth : MonoBehaviour, IHealth
    {
        private BuildingData data;
        private Action onDestroy;

        public bool IsAlive => data.CurrentHP > 0;
        
        public void Init(BuildingData data, Action destroyHandler)
        {
            this.data = data;
            onDestroy = destroyHandler;
        }

        public void RestoreHealth(int health)
        {
            data.CurrentHP = Mathf.Clamp(data.CurrentHP + health, 0, data.MaxHP);
        }

        public void TakeDamage(HitData hitData)
        {
            var damage = ReCalculateDamageWithBonus(hitData);
            data.TakeDamage(damage, onDestroy);
        }

        public int ReCalculateDamageWithBonus(HitData hitData)
        {
            var damageType = hitData.DamageType;
            var armorType = data.ArmorType;
            var finalDamage = hitData.Amount + (hitData.Amount * DamageReduction.GetDamageBonus(damageType, armorType));

            return Mathf.RoundToInt(finalDamage);
        }

        public void RestoreHealth(HitData healData)
        {
            
        }
    }   
}
