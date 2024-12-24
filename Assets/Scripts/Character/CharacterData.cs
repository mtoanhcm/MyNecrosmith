using Config;
using UnityEngine;
using System;

namespace Character
{
    [Serializable]
    public class CharacterData
    {
        protected CharacterConfig baseConfig;
        
        public CharacterID ID => baseConfig.ID;
        public float ViewRadius => baseConfig.ViewRadius;
        public int MaxHP => baseConfig.HP;
        public string MoveSpeedStat => baseConfig.MoveSpeed.ToString();
        public string AttackSpeedStat => baseConfig.AttackSpeed.ToString();
        
        [field: SerializeField]
        public int CurrentHP { get; private set; }

        /// <summary>
        /// Delay between 2 attacks, calculated based on AttackSpeedStat.
        /// If AttackSpeedStat is 100, character can attack 3 times in 1 second.
        /// </summary>
        public float DelayAttack
        {
            get
            {
                if (baseConfig.AttackSpeed <= 0)
                    return 100f; // Prevent division by zero for invalid stats.

                return (baseConfig.AttackSpeed / 100f) * 3f; // 100 stat = 3 attacks per second
            }
        }

        /// <summary>
        /// Movement speed value, calculated based on MoveSpeedStat.
        /// If MoveSpeedStat is 100, the agent can move 1 cell in Unity in 0.5 seconds,
        /// equivalent to moving 2 units per second.
        /// </summary>
        public float RealMoveSpeed
        {
            get
            {
                if (baseConfig.MoveSpeed <= 0)
                    return 0; // Prevent invalid movement speed (e.g., negative or zero).

                // Calculate movement speed in units per second:
                // - At MoveSpeedStat = 100, the agent moves 3 units per second.
                // - MoveSpeedStat scales linearly: higher stat = faster movement.
                return (baseConfig.MoveSpeed / 100f) * 3f; 
            }
        }
        
        public virtual ArmorType ArmorType => ArmorType.Flesh;
        public virtual float AttackRange => 0;

        public CharacterData(CharacterConfig config)
        {
            baseConfig = config;
            CurrentHP = baseConfig.HP;
        }

        public void TakeDamage(int damage, Action onDeathCallback)
        {
            CurrentHP -= damage;
            CurrentHP = Mathf.Clamp(CurrentHP, 0, baseConfig.HP);
            if (CurrentHP <= 0)
            {
                onDeathCallback?.Invoke();
            }
        }

        public void RestoreHealth(int health)
        {
            CurrentHP += health;
            CurrentHP = Mathf.Clamp(CurrentHP, 0, baseConfig.HP);
        }
    }   
}
