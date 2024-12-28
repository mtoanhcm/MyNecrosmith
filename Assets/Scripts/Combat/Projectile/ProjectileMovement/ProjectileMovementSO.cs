using UnityEngine;

namespace Combat
{
    public abstract class ProjectileMovementSO : ScriptableObject
    {
        public abstract void StartMovement(ProjectileController projectile);
    }   
}
