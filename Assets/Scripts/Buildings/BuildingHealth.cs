using InterfaceComp;
using UnityEngine;
using UnityEngine.Events;

namespace Building
{
    public class BuildingHealth : MonoBehaviour, IHealth
    {
        private BuildingData data;
        private UnityAction onDestroy;
        
        public void Init(BuildingData data, UnityAction destroyHandler)
        {
            this.data = data;
            onDestroy = destroyHandler;
        }
        
        public void TakeDamage(int damage)
        {
            data.CurrentHP = Mathf.Clamp(data.CurrentHP - damage, 0, data.MaxHP);
            if (data.CurrentHP <= 0)
            {
                onDestroy?.Invoke();
            }
        }

        public void RestoreHealth(int health)
        {
            data.CurrentHP = Mathf.Clamp(data.CurrentHP + health, 0, data.MaxHP);
        }
    }   
}
