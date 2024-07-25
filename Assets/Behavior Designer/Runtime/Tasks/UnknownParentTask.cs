namespace BehaviorDesigner.Runtime.Tasks
{
    public class UnknownParentTask : ParentTask
    {
        public override void OnAwake()
        {
            base.OnAwake();

            UnityEngine.Debug.LogWarning($"Warning: an unknown task exists with ID {ID}. This task should be replaced.");
        }
    }
}