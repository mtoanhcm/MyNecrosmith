using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameUtility {
    public static class UltilityExtension
    {
        /// <summary>
        /// Determines if the target Transform is within the specified radius of the source Transform
        /// </summary>
        /// <param name="source">The Transform performing the detection.</param>
        /// <param name="target">The Transform to check against.</param>
        /// <param name="radius">The detection radius.</param>
        /// <returns>True if target is within radius; otherwise, false.</returns>
        public static bool IsWithinRadius(this Transform source, Transform target, float radius)
        {
            // Return false if either Transform is null
            if (source == null || target == null) return false;

            // Calculate the squared distance between source and target
            float distanceSquared = (source.position - target.position).sqrMagnitude;

            // Compare squared distance with squared radius for efficiency
            return distanceSquared <= radius * radius;
        }
        
        /// <summary>
        /// Finds the nearest object of type T from the enumerable based on the basePosition.
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour.</typeparam>
        /// <param name="enumerable">Enumerable collection of objects to search.</param>
        /// <param name="basePosition">Reference position to find the nearest object.</param>
        /// <returns>The nearest object of type T. Returns null if the enumerable is empty.</returns>
        public static T FindNearest<T>(this IEnumerable<T> enumerable, Vector3 basePosition) where T : MonoBehaviour
        {
            if (enumerable == null)
            {
                Debug.LogWarning("FindNearest: The enumerable is null.");
                return null;
            }

            T nearest = null;
            float minDistanceSqr = Mathf.Infinity;

            foreach (T obj in enumerable)
            {
                if (obj == null)
                {
                    continue; // Skip null objects
                }

                float distanceSqr = (obj.transform.position - basePosition).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    nearest = obj;
                }
            }

            if (nearest == null)
            {
                Debug.LogWarning("FindNearest: No valid objects found in the enumerable.");
            }

            return nearest;
        }

        public static Vector3 GetRandomNavmeshPositionAround(this Vector3 center, float minRadius, float maxRadius)
        {
            // Generate a random direction on the horizontal plane
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Random distance constrained between minimum and maximum radius
            float randomDistance = Random.Range(minRadius, maxRadius);

            // Calculate the target point in 3D space
            Vector3 targetPoint = center + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;

            // Validate the point on the NavMesh within a tight margin
            if (NavMesh.SamplePosition(targetPoint, out var hit, 1.0f, NavMesh.AllAreas)) // 1.0f margin ensures precision
            {
                return hit.position;
            }

            return Vector3.zero; // Return zero if no valid point is found
        }
        
        public static List<Vector3> GetEquipmentPositionAroundCharacter(this Vector3 characterPosition, int totalEqupiment, 
            float initialRadius = 2f, float radiusIncrement = 2f, float minAngle = 40)
        {
            var positions = new List<Vector3>();
            var currentRadius = initialRadius;
            var maxEquipmentPerCircle = Mathf.FloorToInt(360f / minAngle); // Maximum number of equipment in one circle

            var currentIndex = 0;

            while (currentIndex < totalEqupiment)
            {
                var equipmentsInThisCircle = Mathf.Min(totalEqupiment - currentIndex, maxEquipmentPerCircle); // Number of equipment in the current circle
                var angleStep = 360f / equipmentsInThisCircle;

                for (var i = 0; i < equipmentsInThisCircle; i++)
                {
                    var angle = i * angleStep;
                    var position = new Vector3(
                        Mathf.Cos(angle * Mathf.Deg2Rad) * currentRadius,
                        0,
                        Mathf.Sin(angle * Mathf.Deg2Rad) * currentRadius
                    );

                    positions.Add(characterPosition + position);
                    currentIndex++;
                }

                currentRadius += radiusIncrement; // Increase the radius for the next circle
            }

            return positions;
        }
        
        public static void SetActive(this Component component, bool active)
        {
            if (component == null)
            {
                return;
            }

            if (component.gameObject.activeSelf == active)
            {
                return;
            }
            
            component.gameObject.SetActive(active);
        }
        
        public static bool IsWorldOverlap(this RectTransform originRect, RectTransform targetRect)
        {
            if (targetRect == null)
            {
                return false;
            }
            
            var currentRect1 = GetWorldRect(originRect);
            var currentRect2 = GetWorldRect(targetRect);

            return currentRect1.Overlaps(currentRect2);
            
            Rect GetWorldRect(RectTransform rectTransform)
            {
                var corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                var bottomLeft = corners[0];
                var topRight = corners[2];

                return new Rect(bottomLeft, topRight - bottomLeft);
            }
        }
        
        public static bool IsNulOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
        
        public static bool IsZero(this Vector2 vector) { 
            return vector.x == 0 && vector.y == 0;
        }

        public static bool IsZero(this Vector3 vector)
        {
            return vector.x == 0 && vector.y == 0 && vector.z == 0;
        }
    }
}
