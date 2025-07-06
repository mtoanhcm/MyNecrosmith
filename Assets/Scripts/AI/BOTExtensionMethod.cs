using UnityEngine;
using UnityEngine.AI;

namespace BOT.Extension
{
    public static class BOTExtensionMethod
    {
        /// <summary>
        /// Checks if a NavMesh exists in the current scene.
        /// </summary>
        /// <param name="position">Optional position to check for NavMesh. Defaults to Vector3.zero if not specified.</param>
        /// <param name="maxDistance">Maximum distance to check for NavMesh. Defaults to 1000f.</param>
        /// <returns>True if NavMesh exists, false otherwise.</returns>
        public static bool HasNavMesh(this Vector3 checkPos, float maxDistance = 1f)
        {
            // Try to sample a position on the NavMesh
            bool hasNavMesh = NavMesh.SamplePosition(checkPos, out var hit, maxDistance, NavMesh.AllAreas);
        
            // Additional check: ensure we have actual triangulation data
            if (hasNavMesh && NavMesh.CalculateTriangulation().vertices.Length == 0)
            {
                return false;
            }
        
            return hasNavMesh;
        }
    }   
}
