using Character;
using Config;
using Equipment;
using UnityEngine;

namespace Projectile
{
    public class ProjectileData
    {
        public ProjectileID ID => originConfig.ProjectileID;
        public float Damage { get; private set; }
        public float MoveSpeed { get; private set; }

        private ProjectileDataSO originConfig;

        private EquipmentBase sourceEquipment;
        private CharacterBase sourceCharacter;

        public ProjectileData(ProjectileDataSO config, EquipmentBase equipment, CharacterBase character)
        {
            originConfig = config;
            sourceEquipment = equipment;
            sourceCharacter = character;

            Damage = equipment.Data.Damage;
            MoveSpeed = config.MoveSpeed;
        }

        public void Fire(ProjectileBase projectile)
        {
            originConfig.ProjectileMovement.StartMovement(projectile, ApplyDamage);
        }

        private void ApplyDamage(ProjectileBase projectile)
        {
            originConfig.DamageApplication.DetectAndApplyDamage(projectile);
        }
    }
}
