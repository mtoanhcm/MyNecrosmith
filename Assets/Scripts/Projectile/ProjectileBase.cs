using Combat;
using UnityEngine;

namespace Projectile
{
    public class ProjectileBase : MonoBehaviour
    {
        public ProjectileData Data => data;
        
        private ProjectileData data;

        public void Initialize(AttackData attackData)
        {
            data = new ProjectileData(attackData);
            
            var rotation = Quaternion.LookRotation(attackData.Direction, Vector3.up);
            rotation *= Quaternion.Euler(90f, 0f, 0f);
            
            transform.SetPositionAndRotation(attackData.SpawnPos, rotation);
            data.Fire(this);
        }

        public void ResetData()
        {
            data = null;
        }
    }   
}

