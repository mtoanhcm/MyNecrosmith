using Cysharp.Threading.Tasks;
using Map;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Character.Component {
    public class MovementComponent
    {
        private float speed;

        private List<Vector3Int> path;
        private int currentPathIndex;
        private Tilemap groundTileMap;
        private Transform transform;

        public MovementComponent(Transform transform, Tilemap tileMap, float speed)
        {
            groundTileMap = tileMap;
            this.transform = transform;
            this.speed = speed;
        }

        private async void RunToTarget()
        {
            while (path != null && currentPathIndex < path.Count)
            {
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
        }

        // Method to initiate pathfinding and set the path for the character
        public void FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
        {
            var pathfinding = new PathCalculate(groundTileMap);

            Vector3Int startPos = groundTileMap.WorldToCell(startWorldPos);
            Vector3Int targetPos = groundTileMap.WorldToCell(targetWorldPos);

            path = pathfinding.FindPath(startPos, targetPos);
            currentPathIndex = 0;

            if (path == null) {
                return;
            }

            RunToTarget();
        }
    }
}
