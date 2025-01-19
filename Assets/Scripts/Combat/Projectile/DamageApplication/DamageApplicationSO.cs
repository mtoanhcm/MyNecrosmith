using UnityEngine;

namespace Combat.Projectile
{
    public abstract class DamageApplicationSO : ScriptableObject
    {
        public abstract void DetectAndApplyDamage(ProjectileController projectile);
    }   
}
