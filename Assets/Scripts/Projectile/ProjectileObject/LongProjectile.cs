using UnityEngine;

namespace Projectile
{
    public class LongProjectile : ProjectileBase
    {
        [Header("Damage Area Settings")]
        [SerializeField] private Vector3 damageAreaSize;
        [SerializeField] private Vector3 damageAreaOffset;

        public override (Vector3 center, Vector3 size, Quaternion rotation) GetDamageArea()
        {
            var center = transform.position + transform.rotation * damageAreaOffset;
            return (center, damageAreaSize, transform.rotation);
        }

#if UNITY_EDITOR
        // For visualization in editor
        private void OnDrawGizmos()
        {
            var (center, size, rotation) = GetDamageArea();
        
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, size);
        }
#endif
    }
}
