using Config;
using UnityEngine;
using System;

namespace Character
{
    public class CharacterData
    {
        protected CharacterConfig baseConfig;
        
        public int HP => baseConfig.HP;
        public int CurrentHP { get; private set; }
        public float MoveSpeed => baseConfig.MoveSpeed;
        public float AttackSpeed => baseConfig.AttackSpeed;
        
        public virtual ArmorType ArmorType => ArmorType.Flesh;
        
        public CharacterData(CharacterConfig config)
        {
            baseConfig = config;
            CurrentHP = baseConfig.HP;
        }

        public void TakeDamage(int damage, Action OnDeathCallback)
        {
            CurrentHP -= damage;
            CurrentHP = Mathf.Clamp(CurrentHP, 0, HP);
            if (CurrentHP <= 0)
            {
                OnDeathCallback?.Invoke();
            }
        }
    }   
}
