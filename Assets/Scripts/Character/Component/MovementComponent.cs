using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Component {
    public class MovementComponent
    {
        private float speed;
        private readonly Transform transform;

        private UnityAction endBackCallBack;
        private CancellationTokenSource moveCancellationTokenSource;
        
        public MovementComponent(Transform transform, float speed)
        {
            this.transform = transform;
            this.speed = speed;
        }

        public void UpdateSpeed(float newSpeed) { 
            speed = newSpeed;
        }


        private async void RunToTarget(Vector3 targetPos, CancellationToken cancelToken)
        {
            const float distanceThreshold = 0.2f * 0.2f;
            var isFinisMove = false;
            
            while (!cancelToken.IsCancellationRequested)
            {
                // Move the character towards the target position
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

                // Check if the character has reached the target position
                if ((targetPos - transform.position).magnitude < distanceThreshold)
                {
                    isFinisMove = true;
                    break;
                }

                await UniTask.DelayFrame(1);
            }

            if (isFinisMove)
            {
                endBackCallBack?.Invoke();   
            }
        }

        // Method to initiate pathfinding and set the path for the character
        public void FindPath(Vector3 startWorldPos, Vector3 targetWorldPos, UnityAction onEndPath)
        {
            StopMoving();
            moveCancellationTokenSource = new CancellationTokenSource();
            endBackCallBack = onEndPath;
            RunToTarget(targetWorldPos, moveCancellationTokenSource.Token);
        }

        public void StopMoving()
        {
            moveCancellationTokenSource?.Cancel();
        }
    }
}
