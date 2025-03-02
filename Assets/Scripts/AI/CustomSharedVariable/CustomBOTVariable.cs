using BehaviorDesigner.Runtime;
using Character;
using UnityEngine;

namespace BOT
{
    public class SharedCharacterBase : SharedVariable<CharacterBase>
    {
        public static implicit operator SharedCharacterBase(CharacterBase value) { return new SharedCharacterBase { Value = value }; }
    }
}
