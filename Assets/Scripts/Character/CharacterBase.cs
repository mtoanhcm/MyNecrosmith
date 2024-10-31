using System.Collections;
using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public CharacterStats Stats;

        public virtual void Spawn(CharacterConfig config)
        {
            Stats = new CharacterStats(config);
        }
        
        public virtual void TakeDamage(int damage)
        {
            Stats.TakeDamage(damage, Die);
        }

        protected abstract void Die();
    }   
}
