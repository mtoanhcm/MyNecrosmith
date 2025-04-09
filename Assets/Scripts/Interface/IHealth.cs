using Combat;
using UnityEngine;

namespace InterfaceComp
{
    /// <summary>
    /// Implement to objects that have health stat
    /// </summary>
    public interface IHealth
    {
        public void TakeDamage(HitData hitData);
        public int ReCalculateDamageWithBonus(HitData hitData);
        public void RestoreHealth(HitData healData);
    }
}
