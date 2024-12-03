using System;
using Character;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check Self is Alive", story: "Is [Self] alive", category: "Action/Conditional", id: "15ccfa5e32d3f18fc7b95716f60664de")]
public partial class CheckSelfIsAliveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        var characterHealth = Self.Value.GetComponent<CharacterHealth>();
        if (characterHealth == null)
        {
            return Status.Failure;
        }
        
        return characterHealth.IsAlive ? Status.Success : Status.Failure;
    }
}

