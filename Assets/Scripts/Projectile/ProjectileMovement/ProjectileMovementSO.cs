using System;
using UnityEngine;

namespace Projectile.Movement
{
    public abstract class ProjectileMovementSO : ScriptableObject
    {
        public abstract void StartMovement(ProjectileBase projectile, Func<ProjectileBase, bool> checkApplyDamage);

        protected virtual void CompleteMovement(ProjectileBase projectile)
        {
        }
    }   
}
