using Config;
using Gameplay;
using Observer;
using Sirenix.OdinInspector;
using Spawner;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Building.Tools
{

    public class BuildingPositionDataBehaviour : MonoBehaviour
    {
        public BuildingPositionDataConfig.BuildingPositionData BuildingPositionData => buildingPositionData;

        [SerializeField]
        private BuildingPositionDataConfig.BuildingPositionData buildingPositionData;
        private Vector3 columnSize = new Vector3(1f, 10f, 1f);

        public void SetBuildingPositionData(int areaIndex, Vector3 position, Rarity rarity)
        {
            buildingPositionData = new BuildingPositionDataConfig.BuildingPositionData() {
                AreaIndex = areaIndex,
                PosX = position.x,
                PosY = position.y,
                PosZ = position.z,
                Rarity = rarity 
            };
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(buildingPositionData != null)
            {
                buildingPositionData.PosX = transform.position.x;
                buildingPositionData.PosY = transform.position.y;
                buildingPositionData.PosZ = transform.position.z;
            }
            
            var position = transform.position;
            position.y += 5f;

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(position, columnSize);
            Gizmos.DrawWireCube(position, columnSize);
        }
#endif
    }
    public class BuildingManagerTool : MonoBehaviour
    {
        [Header("Config input")]
        [SerializeField] private BuildingSpawnConfig spawnConfig;

        [Header("Config output")]
        [SerializeField] private BuildingPositionDataConfig buildingPositionDataConfig;

        [Header("Spawn Settings")]
        [SerializeField] private float initialSpawnDelay = 2f;
        [SerializeField] private float spawnDelayBetweenAreas = 1f;
        [SerializeField] private LayerMask buildingLayer;

        [Header("Debug")]
        [SerializeField] private bool drawGizmos = false;
        [SerializeField] private float debugSphereRadius = 1f;
        [SerializeField] private float navMeshSampleDistance = 5f;

        // Dictionary to track spawned buildings by area
        private Dictionary<int, List<BuildingPositionDataBehaviour>> spawnedBuildingPositions = new Dictionary<int, List<BuildingPositionDataBehaviour>>();
        // Dictionary to track failed positions for debugging
        private Dictionary<int, List<(Vector3 position, string reason)>> failedPositions = new Dictionary<int, List<(Vector3, string)>>();
        // Track area spawn stats
        private Dictionary<int, (int attempted, int succeeded)> areaStats = new Dictionary<int, (int, int)>();
        private List<BuildingPositionDataConfig.BuildingPositionData> spawnedBuildingDatas = new List<BuildingPositionDataConfig.BuildingPositionData>();
        private void OnDestroy()
        {
            ClearAllPositionObject();
        }

        [Button]
        public void GenerateNewBuildingPosition()
        {
            ClearAllPositionObject();
            SpawnInitialBuildings();
        }

        [Button]
        public void ExtractBuildingPositionsToConfig()
        {
            buildingPositionDataConfig.SetBuildingPositionData(spawnedBuildingDatas);
        }

        [Button]
        public void LoadBuildingPositionData()
        {

        }

        [Button]
        public void ClearAllPositionObject()
        {
            if(spawnedBuildingPositions.Count > 0)
            {
                foreach (var item in spawnedBuildingPositions)
                {
                    for (var i = 0; i < item.Value.Count; i++)
                    {
                        DestroyImmediate(item.Value[i].gameObject);
                    }
                }
            } else
            {
                var childObject = transform.GetComponentsInParent<BuildingPositionDataBehaviour>();
                for(var i = 0; i < childObject.Length; i++)
                {
                    DestroyImmediate(childObject[i].gameObject);
                }
            }

            spawnedBuildingPositions.Clear();
            spawnedBuildingDatas.Clear();
            failedPositions.Clear();
            areaStats.Clear();
        }

        /// <summary>
        /// Spawns buildings for all areas with a delay between each area
        /// </summary>
        private void SpawnInitialBuildings()
        {
            //Spawn building for each area
            var maxArea = spawnConfig.AreaConfigs.Length;
            for (int areaIndex = 0; areaIndex < maxArea; areaIndex++)
            {
                CreateBuildingsSpawnPosition(areaIndex);
            }
        }

        /// <summary>
        /// Spawns buildings for a specific area
        /// </summary>
        private void CreateBuildingsSpawnPosition(int areaIndex)
        {
            var areaConfig = spawnConfig.GetAreaConfig(areaIndex);
            if (areaConfig == null)
            {
                return;
            }

            if (areaConfig.EnemyBuildingConfigs == null)
            {
                return;
            }

            // Initialize positions tracker for this area if needed
            if (!spawnedBuildingPositions.ContainsKey(areaIndex))
            {
                spawnedBuildingPositions[areaIndex] = new List<BuildingPositionDataBehaviour>();
            }

            // Initialize failed positions tracker
            if (!failedPositions.ContainsKey(areaIndex))
            {
                failedPositions[areaIndex] = new List<(Vector3, string)>();
            }

            int attemptedBuildings = 0;
            int successfullySpawned = 0;
            int maxAttempts = areaConfig.BuildingAmount * 10; // Limit attempts to avoid infinite loop

            while (successfullySpawned < areaConfig.BuildingAmount && attemptedBuildings < maxAttempts)
            {
                attemptedBuildings++;

                // Get random position within the area's range
                Vector3 spawnPosition = GetRandomPositionInArea(areaConfig, areaIndex);

                // Check if position is valid for building placement
                if (IsValidBuildingPosition(spawnPosition, areaIndex, areaConfig.BuildingDistance, out var failReason))
                {
                    // Get random enemy building config from available options
                    //EnemyBuildingConfig buildingConfig = GetRandomEnemyBuildingConfig(areaConfig, areaIndex);
                    //if (buildingConfig == null)
                    //{
                    //    failedPositions[areaIndex].Add((spawnPosition, "Null building config"));
                    //    continue;
                    //}

                    try
                    {
                        var tempObj = new GameObject(spawnPosition.ToString());
                        tempObj.transform.position = spawnPosition;
                        tempObj.transform.SetParent(transform);

                        var buildingPositionData = tempObj.AddComponent<BuildingPositionDataBehaviour>();
                        buildingPositionData.SetBuildingPositionData(areaIndex, spawnPosition, Rarity.Common);
                        spawnedBuildingDatas.Add(buildingPositionData.BuildingPositionData);
                        // Track the position
                        spawnedBuildingPositions[areaIndex].Add(buildingPositionData);

                        successfullySpawned++;
                    }
                    catch (System.Exception e)
                    {
                        failedPositions[areaIndex].Add((spawnPosition, $"Exception: {e.Message}"));
                    }
                }
                else
                {
                    failedPositions[areaIndex].Add((spawnPosition, failReason));
                }
            }

            // Store spawn stats for this area
            areaStats[areaIndex] = (attemptedBuildings, successfullySpawned);

            if (successfullySpawned < areaConfig.BuildingAmount)
            {
                if (failedPositions[areaIndex].Count > 0)
                {
                    Dictionary<string, int> reasonCounts = new Dictionary<string, int>();
                    foreach (var (_, reason) in failedPositions[areaIndex])
                    {
                        if (!reasonCounts.ContainsKey(reason))
                            reasonCounts[reason] = 0;
                        reasonCounts[reason]++;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a random position within the area's range
        /// </summary>
        private Vector3 GetRandomPositionInArea(BuildingSpawnConfig.AreaBuildingData areaDataConfig, int areaIndex)
        {
            // Get random direction
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Get random distance within range
            float distance = Random.Range(areaDataConfig.SpawnRangeMin, areaDataConfig.SpawnRangeMax);

            // Calculate position
            Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * distance;

            // Ensure the point is on the NavMesh
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                //LogDebug($"Area {areaIndex}: Found NavMesh at {hit.position}, distance from original={Vector3.Distance(randomPosition, hit.position)}");
                return hit.position;
            }

            //LogWarning($"Area {areaIndex}: No NavMesh found near {randomPosition} within {navMeshSampleDistance} units!");
            return randomPosition;
        }

        /// <summary>
        /// Checks if the position is valid for building placement
        /// </summary>
        private bool IsValidBuildingPosition(Vector3 position, int areaIndex, float minDistance, out string failReason)
        {
            failReason = "";

            // Check that the position is on NavMesh
            // if (!NavMesh.SamplePosition(position, out _, 0.1f, NavMesh.AllAreas))
            // {
            //     failReason = "Position not on NavMesh";
            //     return false;
            // }

            // Check against existing buildings in the same area
            if (spawnedBuildingPositions.TryGetValue(areaIndex, out var positions))
            {
                foreach (var existingPosData in positions)
                {
                    float distance = Vector3.Distance(position, existingPosData.BuildingPositionData.Position);
                    if (distance < minDistance)
                    {
                        failReason = $"Too close to existing building ({distance:F2} < {minDistance})";
                        return false;
                    }
                }
            }

            // Check against physical collisions with existing buildings
            Collider[] colliders = Physics.OverlapSphere(position, 2f, buildingLayer);
            if (colliders.Length > 0)
            {
                string colliderNames = "";
                for (int i = 0; i < Mathf.Min(colliders.Length, 3); i++)
                {
                    colliderNames += colliders[i].name + " ";
                }
                failReason = $"Colliders detected: {colliderNames.Trim()}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a random enemy building config from available options
        /// </summary>
        private EnemyBuildingConfig GetRandomEnemyBuildingConfig(BuildingSpawnConfig.AreaBuildingData areaDataConfig, int areaIndex)
        {
            if (areaDataConfig.EnemyBuildingConfigs == null || areaDataConfig.EnemyBuildingConfigs.Length == 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, areaDataConfig.EnemyBuildingConfigs.Length);
            var config = areaDataConfig.EnemyBuildingConfigs[randomIndex];

            if (config == null)
            {
                return null;
            }

            return config;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!drawGizmos || spawnConfig == null) return;

            // Draw area ranges
            foreach (var areaConfig in spawnConfig.AreaConfigs)
            {
                // Draw min radius filled circle
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                DrawFilledCircle(transform.position, areaConfig.SpawnRangeMin, 32);

                // Draw max radius filled circle
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                DrawFilledCircle(transform.position, areaConfig.SpawnRangeMax, 32);
            }

            // Draw spawned building positions
            Gizmos.color = Color.blue;
            foreach (var kvp in spawnedBuildingPositions)
            {
                foreach (var posData in kvp.Value)
                {
                    Gizmos.DrawSphere(posData.BuildingPositionData.Position, debugSphereRadius);
                }
            }

            // Draw failed positions in red
            Gizmos.color = Color.red;
            if (failedPositions != null)
            {
                foreach (var kvp in failedPositions)
                {
                    foreach (var (position, _) in kvp.Value)
                    {
                        Gizmos.DrawSphere(position, debugSphereRadius * 0.5f);
                    }
                }
            }
        }

        private void DrawFilledCircle(Vector3 center, float radius, int segments)
        {
            // Draw filled circle using triangles
            float angleStep = 360f / segments;
            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * angleStep * Mathf.Deg2Rad;
                float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

                Vector3 point1 = center;
                Vector3 point2 = center + new Vector3(Mathf.Cos(angle1) * radius, 0, Mathf.Sin(angle1) * radius);
                Vector3 point3 = center + new Vector3(Mathf.Cos(angle2) * radius, 0, Mathf.Sin(angle2) * radius);

                // Draw triangle from center to two points on the circle
                UnityEditor.Handles.color = Gizmos.color;
                UnityEditor.Handles.DrawAAConvexPolygon(point1, point2, point3);
            }

            // Draw circle outline
            DrawCircle(center, radius, segments);
        }

        private void DrawCircle(Vector3 center, float radius, int segments)
        {
            // Draw circle outline by connecting points along the circumference
            float angleStep = 360f / segments;
            Vector3 previousPoint = center + new Vector3(Mathf.Cos(0) * radius, 0, Mathf.Sin(0) * radius);
            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }
#endif
    }
}
