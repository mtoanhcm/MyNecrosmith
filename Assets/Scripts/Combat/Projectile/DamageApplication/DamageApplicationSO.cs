using UnityEngine;

namespace Combat
{
    public abstract class DamageApplicationSO : ScriptableObject
    {
        public abstract void DetectAndApplyDamage(GameObject projectile);
    }   
}
