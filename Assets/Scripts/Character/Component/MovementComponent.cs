using Cysharp.Threading.Tasks;
using Map;
using Pathfinding;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Character.Component {
    public class MovementComponent
    {
        private float speed;

        private List<Vector3Int> path;
        private int currentPathIndex;
        private readonly Tilemap groundTileMap;
        private readonly Transform transform;

        private UnityAction endBackCallBack;
        private PathCalculate pathCalculate;

        public MovementComponent(Transform transform, Tilemap tileMap, float speed)
        {
            groundTileMap = tileMap;
            this.transform = transform;
            this.speed = speed;
            pathCalculate = new(tileMap);
        }

        private async void RunToTarget()
        {
            while (path != null && currentPathIndex < path.Count)
            {
                if (transform == null) {
                    break;
                }

                // Use GetCellCenterWorld to get the center position of the current path tile in world coordinates
                Vector3 targetPosition = groundTileMap.GetCellCenterWorld(path[currentPathIndex]);
                // Move the character towards the target position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Check if the character has reached the target position
                if ((targetPosition - transform.position).magnitude < 0.1f * 0.1f)
                {
                    currentPathIndex++;
                }

                await UniTask.DelayFrame(1);
            }

            endBackCallBack?.Invoke();
        }

        // Method to initiate pathfinding and set the path for the character
        public void FindPath(Vector3 startWorldPos, Vector3 targetWorldPos, UnityAction onEndPath)
        {
            Vector3Int startPos = groundTileMap.WorldToCell(startWorldPos);
            Vector3Int targetPos = groundTileMap.WorldToCell(targetWorldPos);

            path = pathCalculate.FindPath(startPos, targetPos);
            currentPathIndex = 0;

            if (path == null) {
                return;
            }

            endBackCallBack = onEndPath;

            RunToTarget();
        }
    }
}
