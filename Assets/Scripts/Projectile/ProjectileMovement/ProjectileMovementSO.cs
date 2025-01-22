using UnityEngine;
using UnityEngine.Events;

namespace Projectile
{
    public abstract class ProjectileMovementSO : ScriptableObject
    {
        public abstract void StartMovement(ProjectileBase projectile, UnityAction<ProjectileBase> checkApplyDamage);
    }   
}
