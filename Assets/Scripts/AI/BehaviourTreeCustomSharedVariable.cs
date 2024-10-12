using BehaviorDesigner.Runtime;
using Character;
using System;
using Building;

namespace AI {

    [Serializable]
    public class SharedCharacterBase : SharedVariable<CharacterBase>
    {
        public static implicit operator SharedCharacterBase(CharacterBase value) { return new SharedCharacterBase { Value = value }; }
    }
    
    [Serializable]
    public class SharedBuildingBase : SharedVariable<BuildingBase>
    {
        public static implicit operator SharedBuildingBase(BuildingBase value) { return new SharedBuildingBase { Value = value }; }
    }
}
