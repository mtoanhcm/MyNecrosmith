using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "Projectile/ProjectileData")]
    public class ProjectileDataSO : ScriptableObject
    {
        [Header("Visual / Prefab")]
        public GameObject ProjectilePrefab;
        
        [Header("Movement Logic")]
        public ProjectileMovementSO ProjectileMovement;
        
        [Header("Damage Logic")]
        public DamageApplicationSO DamageApplication;
    }   
}
