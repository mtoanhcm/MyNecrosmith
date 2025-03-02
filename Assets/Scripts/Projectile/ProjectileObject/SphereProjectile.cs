using UnityEngine;

namespace Projectile
{
    public class SphereProjectile : ProjectileBase
    {
        [SerializeField] private float damageRadius = 1f;
        [SerializeField] private Vector3 damageAreaOffset = Vector3.zero;
        
        public override (Vector3 center, Vector3 size, Quaternion rotation) GetDamageArea()
        {
            var center = transform.position + transform.rotation * damageAreaOffset;
            var size = Vector3.one * (damageRadius * 2);
            return (center, size, transform.rotation);
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var center = transform.position + transform.rotation * damageAreaOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, damageRadius);
        }
        #endif
    }
}
