using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Ultility;
using UnityEngine;

namespace AI
{
    [TaskCategory("AI_V1")]
    [TaskDescription("Check have target construct in detect range")]
    public class BOTHasConstructInRange : Conditional
    {
        [Header("------ Output ------")] private SharedBuildingBase targetBuilding;
        
        private BotBrain brain;

        public override void OnAwake()
        {
            brain = GetComponent<BotBrain>();
        }

        public override TaskStatus OnUpdate()
        {
            var buildings = brain.Character.GetBuildingAround();
            if (buildings.Length == 0)
            {
                targetBuilding.Value = null;
                targetBuilding.Value = null;
                return TaskStatus.Failure;
            }

            var building = buildings.FindNearest(transform);
            targetBuilding.Value = building != null ? building : null;
            
            return TaskStatus.Success;
            
        }
    }   
}
