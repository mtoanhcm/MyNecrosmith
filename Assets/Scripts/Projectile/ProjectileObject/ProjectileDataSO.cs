using Config;
using Projectile.DamageApply;
using Projectile.Movement;
using UnityEngine;

namespace Projectile
{
    [CreateAssetMenu(menuName = "baseConfig/Projectile/ProjectileDataSO")]
    public class ProjectileDataSO : ScriptableObject
    {
        [Header("Stat")]
        public ProjectileID ProjectileID;
        
        [Header("Movement Logic")]
        public ProjectileMovementSO ProjectileMovement;
        
        [Header("Damage Logic")]
        public DamageApplicationSO DamageApplication;
    }   
}
