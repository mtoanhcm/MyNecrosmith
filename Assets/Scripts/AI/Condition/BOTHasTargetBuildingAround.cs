using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using GameUtility;
using UnityEngine;

namespace BOT
{
    [TaskCategory("BOT_V1")]
    [TaskDescription("Check has target building around")]
    public class BOTHasTargetBuildingAround : Conditional
    {
        [SerializeField] private SharedCharacterBase character;
        
        [Header("--------- Output ----------")]
        [SerializeField] private SharedBuildingBase targetBuilding;
        [SerializeField] private SharedTransform targetTransform;
        
        public override TaskStatus OnUpdate()
        {
            return HasTargetBuildingAround() ? TaskStatus.Success : TaskStatus.Failure;
        }
        
        private bool HasTargetBuildingAround()
        {
            var targetBuildingAround = character.Value.CharacterBrain.GetTargetBuildingAround();
            if (targetBuildingAround.Length == 0)
            {
                targetBuilding.Value = null;
                targetTransform.Value = null;
                return false;
            }

            var building = targetBuildingAround.FindNearest(character.Value.transform.position);
            if(building == null) 
            {
                return false;
            }

            targetBuilding.Value = building;
            targetTransform.Value = targetBuilding.Value.transform;
            return true;
        }
    }   
}
