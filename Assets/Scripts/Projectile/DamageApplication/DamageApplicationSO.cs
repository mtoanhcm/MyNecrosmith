using UnityEngine;

namespace Projectile
{
    public abstract class DamageApplicationSO : ScriptableObject
    {
        public abstract void DetectAndApplyDamage(ProjectileBase projectile);
    }   
}
