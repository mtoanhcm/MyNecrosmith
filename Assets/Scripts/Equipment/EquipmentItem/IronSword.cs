using Character;
using Config;
using UnityEngine;

namespace Equipment
{
    public class IronSword : EquipmentBase
    {
        private FlyFowardAction flyTowardAction;

        public override void Spawn(CharacterBase owner ,EquipmentConfig equipmentConfig)
        {
            base.Spawn(owner ,equipmentConfig);
            
            flyTowardAction = new FlyFowardAction();
        }

        public override void PerformAction(Transform target)
        {
            flyTowardAction.Execute(Owner,target,this);
        }

        public override void DeSpawn()
        {
            
        }
    }   
}
