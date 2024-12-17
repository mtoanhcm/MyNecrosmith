using BehaviorDesigner.Runtime.Tasks;
using Character;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Check main character is alive or not")]
    public class BOTIsAlive : Conditional
    {
        [SerializeField]
        private CharacterBrain brain;
        private TaskStatus status;
        
        public override void OnAwake()
        {
            brain = GetComponent<CharacterBrain>();
        }

        public override void OnStart()
        {
            Debug.Log("AAAA");
            status = brain.IsCharacterAlive ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override TaskStatus OnUpdate()
        {
            return status;
        }
    }   
}
