using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
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

        protected abstract void Die();
    }   
}
