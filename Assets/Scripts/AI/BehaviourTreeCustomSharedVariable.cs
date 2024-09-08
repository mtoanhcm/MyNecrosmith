using BehaviorDesigner.Runtime;
using Character;
using System;

namespace AI {

    [Serializable]
    public class SharedCharacterBase : SharedVariable<CharacterBase>
    {
        public static implicit operator SharedCharacterBase(CharacterBase value) { return new SharedCharacterBase { Value = value }; }
    }
}
