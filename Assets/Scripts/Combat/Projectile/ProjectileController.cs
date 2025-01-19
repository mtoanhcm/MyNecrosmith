using Character;
using Equipment;
using UnityEngine;

namespace Combat.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        private ProjectileData data;

        public void Initialize(EquipmentBase equipmentSource, CharacterBase characterSource)
        {
            data = new ProjectileData(equipmentSource.Data.WeaponConfig.ProjectileDataConfig, equipmentSource, characterSource);
            
            CreateVFX();
            
            data.Fire(this);
        }

        private void CreateVFX()
        {
            //TODO: create VFX   
        }
    }   
}

