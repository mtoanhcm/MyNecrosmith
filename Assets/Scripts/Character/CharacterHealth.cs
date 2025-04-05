using UnityEngine;
using InterfaceComp;
using UnityEngine.Events;

namespace Character
{
    public class CharacterHealth : MonoBehaviour, IHealth
    {
        public bool IsAlive => localCharacter != null && localCharacter.Data.CurrentHP > 0;

        private CharacterBase localCharacter;
        private UnityAction onDeath;

        public void Init(CharacterBase character, UnityAction deathHandler)
        {
            localCharacter = character;
            onDeath = deathHandler;
        }
        
        public void TakeDamage(int damage)
        {
            if (localCharacter == null)
            {
                Debug.LogError($"Null local character {gameObject.name}");
                return;
            }

            localCharacter.Data.TakeDamage(damage, Die);
        }

        public void RestoreHealth(int health)
        {
            if (localCharacter == null)
            {
                Debug.LogError($"Null local character {gameObject.name}");
                return;
            }
            
            localCharacter.Data.RestoreHealth(health);
        }

        private void Die()
        {
            onDeath?.Invoke();
        }
    }
}
