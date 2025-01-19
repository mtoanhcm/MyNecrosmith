using UnityEngine;
using UnityEngine.Events;

namespace Combat.Projectile
{
    public abstract class ProjectileMovementSO : ScriptableObject
    {
        public abstract void StartMovement(ProjectileController projectile, UnityAction<ProjectileController> checkApplyDamage);
    }   
}
