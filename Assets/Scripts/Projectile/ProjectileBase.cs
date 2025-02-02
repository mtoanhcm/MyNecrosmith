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
            data.Fire(this);
        }

        public void ResetData()
        {
            data = null;
        }
    }   
}

