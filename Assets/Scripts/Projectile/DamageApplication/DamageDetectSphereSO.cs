using GameUtility;
using InterfaceComp;
using UnityEngine;

namespace Projectile.DamageApply
{
    [CreateAssetMenu(fileName = "DamageDetectSphereSO", menuName = "baseConfig/Projectile/DamageDetect/SphereDetect")]
    public class DamageDetectSphereSO : DamageApplicationSO
    {
        private Collider[] hits = new Collider[1];
        
        public override bool DetectAndApplyDamage(ProjectileBase projectile)
        {
            var (center, size, _) = projectile.GetDamageArea();
            // Since size is diameter (from SphereProjectile), we need radius which is size.x/2
            var hitCount = Physics.OverlapSphereNonAlloc(center, size.x / 2, hits, projectile.GetTargetLayer());
            if (hitCount == 0)
            {
                return false;
            }
        
            Debug.Log($"AAA {hits[0]}");
            if (hits[0].TryGetComponent<IHealth>(out var hitObj))
            {
                hitObj.TakeDamage(projectile.Data.Damage);
            }
            
            return true;
        }
    }   
}
