using UnityEngine;
using UnityEngine.Events;

namespace Projectile.Movement
{
    public abstract class ProjectileMovementSO : ScriptableObject
    {
        public abstract void StartMovement(ProjectileBase projectile, UnityAction<ProjectileBase> checkApplyDamage);
    }   
}
