using System.Collections;
using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public CharacterStats Stats { get;private set; }

        private Transform modelContainer;
        
        public virtual void Spawn(CharacterStats stat)
        {
            Stats = stat;
        }
        
        public virtual void TakeDamage(int damage)
        {
            Stats.TakeDamage(damage, Die);
        }

        protected abstract void Die();
    }   
}
