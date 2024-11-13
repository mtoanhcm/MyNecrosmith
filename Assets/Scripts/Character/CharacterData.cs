using Config;
using UnityEngine;
using System;

namespace Character
{
    public class CharacterStats
    {
        private CharacterConfig baseConfig;
        
        public C_Class Class => baseConfig.Class;
        public int HP => baseConfig.HP;
        public int CurrentHP { get; private set; }
        public float MoveSpeed => baseConfig.MoveSpeed;
        public float AttackSpeed => baseConfig.AttacSpeed;
        
        public CharacterStats(CharacterConfig config)
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

    public enum C_Class
    {
        HumanKnight
    }

    public enum C_Race
    {
        Human
    }
}
