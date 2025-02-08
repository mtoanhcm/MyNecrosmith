using UnityEngine;

namespace Projectile.DamageApply
{
    public abstract class DamageApplicationSO : ScriptableObject
    {
        public abstract bool DetectAndApplyDamage(ProjectileBase projectile);
    }   
}
