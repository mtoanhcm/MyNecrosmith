using Character;
using Combat;
using Equipment;
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
    }   
}

