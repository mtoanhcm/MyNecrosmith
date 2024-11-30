using InterfaceComp;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour, IHealth
    {
        public CharacterData Data { get;private set; }

        private Transform modelContainer;
        
        public virtual void Spawn(CharacterData data)
        {
            Data = data;
        }
        
        public virtual void TakeDamage(int damage)
        {
            Data.TakeDamage(damage, Die);
        }

        public virtual void RestoreHealth(int health)
        {
            Data.RestoreHealth(health);
        }

        protected abstract void Die();
    }   
}
