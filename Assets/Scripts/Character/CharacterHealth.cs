using UnityEngine;
using InterfaceComp;
using UnityEngine.Events;

namespace Character
{
    public class CharacterHealth : MonoBehaviour, IHealth
    {
        public bool IsAlive => localCharacter != null && localCharacter.Data.CurrentHP > 0;

        [SerializeField]
        private CharacterBase localCharacter;
        private UnityAction onDeath;

        public void Init(CharacterBase character, UnityAction deathHandler)
        {
            Debug.Log($"Set local character {character.name}");
            localCharacter = character;
            onDeath = deathHandler;
        }
        
        public void TakeDamage(int damage)
        {
            if (localCharacter == null)
            {
                Debug.Log($"Null local character {gameObject.name}");
            }

            Debug.Log($"{localCharacter.name} takes damage");
            localCharacter.Data.TakeDamage(damage, Die);
        }

        public void RestoreHealth(int health)
        {
            localCharacter.Data.RestoreHealth(health);
        }

        private void Die()
        {
            onDeath?.Invoke();
        }
    }
}
