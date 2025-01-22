using Character;
using Equipment;
using UnityEngine;

namespace Projectile
{
    public class ProjectileBase : MonoBehaviour
    {
        public ProjectileData Data => data;
        
        private ProjectileData data;

        public void Initialize(EquipmentBase equipmentSource, CharacterBase characterSource)
        {
            data = new ProjectileData(equipmentSource.Data.WeaponConfig.ProjectileDataConfig, equipmentSource, characterSource);
            data.Fire(this);
        }
    }   
}

