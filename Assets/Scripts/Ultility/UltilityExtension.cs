using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ultility {
    public static class UltilityExtension
    {
        public static T FindNearest<T>(this IEnumerable<T> objects, Transform referencePoint) where T : MonoBehaviour
        {
            T nearestObject = null;
            float minDistanceSqr = Mathf.Infinity;

            // Cache the position of the reference point (e.g., the player's position)
            Vector3 referencePosition = referencePoint.position;

            foreach (T obj in objects)
            {
                // Avoid null references, make sure the object exists
                if (obj == null) continue;

                // Calculate the squared distance between the reference point and the current object
                float distanceSqr = (referencePosition - obj.transform.position).sqrMagnitude;

                // Update the nearest object if a closer one is found
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    nearestObject = obj;
                }
            }

            return nearestObject;
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
