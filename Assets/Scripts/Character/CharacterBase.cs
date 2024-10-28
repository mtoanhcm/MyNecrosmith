using System.Collections;
using System.Collections.Generic;
using Config;
using Equipment;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public float HP { get; protected set; }
        public float CurrentHP { get; protected set; }
        public float MoveSpeed { get; protected set; }

        public virtual void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            // Implement death logic.
        }
    }   
}
