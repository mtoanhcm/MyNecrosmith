using Character;
using Equipment;
using UnityEngine;

namespace Combat.Projectile
{
    public class ProjectileData
    {
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

        public void Fire(ProjectileController projectile)
        {
            originConfig.ProjectileMovement.StartMovement(projectile, ApplyDamage);
        }

        private void ApplyDamage(ProjectileController projectile)
        {
            originConfig.DamageApplication.DetectAndApplyDamage(projectile);
        }
    }
}
