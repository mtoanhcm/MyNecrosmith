using Character;
using UnityEngine;

namespace Equipment
{
    public abstract class ActionBase
    {
        public abstract void Execute(CharacterBase caster, Transform target, EquipmentBase equipment);
    }   
}
