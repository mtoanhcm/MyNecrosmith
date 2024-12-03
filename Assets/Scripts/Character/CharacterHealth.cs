using UnityEngine;
using InterfaceComp;

namespace Character
{
    public class CharacterHealth : MonoBehaviour, IHealth
    {
        public bool IsAlive => localCharacter != null && localCharacter.Data.CurrentHP > 0;

        private CharacterBase localCharacter;

        public void Init(CharacterBase character)
        {
            this.localCharacter = character;
        }
        
        public void TakeDamage(int damage)
        {
            localCharacter.Data.TakeDamage(damage, Die);
        }

        public void RestoreHealth(int health)
        {
            localCharacter.Data.RestoreHealth(health);
        }

        private void Die()
        {
            
        }
    }
}
