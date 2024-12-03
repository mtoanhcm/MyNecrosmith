using UnityEngine;
using InterfaceComp;

namespace Character
{
    public class CharacterHealth : MonoBehaviour, IHealth
    {
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
