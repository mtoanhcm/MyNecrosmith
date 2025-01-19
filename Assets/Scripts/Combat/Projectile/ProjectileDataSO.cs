using Config;
using UnityEngine;

namespace Combat.Projectile
{
    [CreateAssetMenu(menuName = "Projectile/ProjectileData")]
    public class ProjectileDataSO : ScriptableObject
    {
        [Header("Stat")]
        public ProjectileID ProjectileID;
        public float MoveSpeed;
        
        [Header("Movement Logic")]
        public ProjectileMovementSO ProjectileMovement;
        
        [Header("Damage Logic")]
        public DamageApplicationSO DamageApplication;
    }   
}
