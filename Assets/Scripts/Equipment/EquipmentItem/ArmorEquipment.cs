using Character;
using UnityEngine;

namespace Equipment.Armor
{
    public class ArmorEquipment : EquipmentBase
    {
        private ArmorData armorData;

        public override void Init(CharacterBase owner, EquipmentData data, Vector2 spawnPosition)
        {
            base.Init(owner, data, spawnPosition);

            armorData = data as ArmorData;
        }

        public override void PerformAction(Transform target)
        {
            
        }
    }

}