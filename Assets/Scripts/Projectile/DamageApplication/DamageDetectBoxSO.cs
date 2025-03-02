using GameUtility;
using InterfaceComp;
using UnityEngine;

namespace Projectile.DamageApply
{
    [CreateAssetMenu(fileName = "DamageDetectBoxSO", menuName = "baseConfig/Projectile/DamageDetect/BoxDetect")]
    public class DamageDetectBoxSO : DamageApplicationSO
    {
        private Collider[] hits = new Collider[1];
        
        public override bool DetectAndApplyDamage(ProjectileBase projectile)
        {
            var (center, size, rotation) = projectile.GetDamageArea();
        
            var hitCount = Physics.OverlapBoxNonAlloc(center, size / 2, hits, rotation, projectile.GetTargetLayer());
            if (hitCount == 0)
            {
                return false;
            }
            
            if (hits[0].TryGetComponent<IHealth>(out var hitObj))
            {
                hitObj.TakeDamage(projectile.Data.Damage);
            }
                
            return true;
        }
    }
}
