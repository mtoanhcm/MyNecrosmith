using CodeMonkey.Utils;
using Combat;
using UnityEngine;
using InterfaceComp;
using System;

namespace Character
{
    public class CharacterHealth : MonoBehaviour, IHealth
    {
        public bool IsAlive => localCharacter != null && localCharacter.Data.CurrentHP > 0;

        private CharacterBase localCharacter;
        private Action onDeath;

        public void Init(CharacterBase character, Action deathHandler)
        {
            localCharacter = character;
            onDeath = deathHandler;
        }
        
        public void TakeDamage(HitData hitData)
        {
            if (localCharacter == null)
            {
                Debug.LogError($"Null local character {gameObject.name}");
                return;
            }
            
            var damage = ReCalculateDamageWithBonus(hitData);
            
            UtilsClass.CreateWorldTextPopup(damage.ToString(), transform.position, 1f);
            if (!Cheat.DeveloperMode.IsImmortal)
            {
                localCharacter.Data.TakeDamage(damage, onDeath);
            }
        }

        public int ReCalculateDamageWithBonus(HitData hitData)
        {
            var damageType = hitData.DamageType;
            var armorType = localCharacter.Data.ArmorType;
            var finalDamage = hitData.Amount + (hitData.Amount * DamageReduction.GetDamageBonus(damageType, armorType));

            return Mathf.RoundToInt(finalDamage);
        }

        public void RestoreHealth(HitData healData)
        {
            if (localCharacter == null)
            {
                Debug.LogError($"Null local character {gameObject.name}");
                return;
            }
            
            localCharacter.Data.RestoreHealth(healData);
        }
    }
}
