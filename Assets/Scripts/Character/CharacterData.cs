using Config;
using UnityEngine;

namespace Character
{
    public class CharacterStats
    {
        public int HP { get; private set; }
        public int CurrentHP { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackSpeed { get; private set; }

        public CharacterStats(CharacterConfig config)
        {
            HP = config.HP;
            CurrentHP = HP;
            MoveSpeed = config.MoveSpeed;
            AttackSpeed = config.AttacSpeed;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            CurrentHP = Mathf.Clamp(CurrentHP, 0, HP);
        }
    }

    public enum C_Class
    {
        SwordMan
    }

    public enum C_Race
    {
        Human
    }
}
