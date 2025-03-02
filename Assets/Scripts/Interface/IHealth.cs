using UnityEngine;

namespace InterfaceComp
{
    /// <summary>
    /// Implement to objects that have health stat
    /// </summary>
    public interface IHealth
    {
        public void TakeDamage(int damage);
        public void RestoreHealth(int health);
    }
}
