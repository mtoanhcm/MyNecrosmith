using BehaviorDesigner.Runtime.Tasks;
using Character;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Check main character is alive or not")]
    public class BOTIsAlive : Conditional
    {
        [SerializeField]
        private SharedCharacterBase character;
        private TaskStatus status;

        public override TaskStatus OnUpdate()
        {
            status = character.Value.CharacterBrain.IsCharacterAlive ? TaskStatus.Success : TaskStatus.Failure;
            return status;
        }
    }   
}
