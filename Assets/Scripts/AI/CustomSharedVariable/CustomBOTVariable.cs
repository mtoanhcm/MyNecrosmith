using BehaviorDesigner.Runtime;
using Building;
using Character;
using UnityEngine;

namespace BOT
{
    public class SharedCharacterBase : SharedVariable<CharacterBase>
    {
        public static implicit operator SharedCharacterBase(CharacterBase value) { return new SharedCharacterBase { Value = value }; }
    }
    
    public class SharedBuildingBase : SharedVariable<BuildingBase>
    {
        public static implicit operator SharedBuildingBase(BuildingBase value) { return new SharedBuildingBase { Value = value }; }
    }
}
