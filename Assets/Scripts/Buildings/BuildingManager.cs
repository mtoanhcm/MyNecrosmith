using System.Collections;
using System.Collections.Generic;
using Config;
using Observer;
using Sirenix.OdinInspector;
using Spawner;
using UnityEngine;
using UnityEngine.AI;

namespace Building
{
    public class BuildingManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private BuildingSpawnConfig spawnConfig;
        [SerializeField] private BuildingSpawner buildingSpawner;
        [SerializeField] private Transform mapCenter;
        
        [Header("Spawn Settings")]
        [SerializeField] private float initialSpawnDelay = 2f;
        [SerializeField] private float spawnDelayBetweenAreas = 1f;
        [SerializeField] private int amountBuildingActiveInFirstWave = 5;
        [SerializeField] private int delayTimeActiveTheFirstWave = 20;
        [SerializeField] private LayerMask buildingLayer;
        
        [Header("Debug")]
        [SerializeField] private bool drawGizmos = false;
        [SerializeField] private float debugSphereRadius = 1f;
        [SerializeField] private float navMeshSampleDistance = 5f;

        // Dictionary to track spawned buildings by area
        private Dictionary<int, List<Vector3>> spawnedBuildingPositions = new Dictionary<int, List<Vector3>>();
        // Dictionary to track failed positions for debugging
        private Dictionary<int, List<(Vector3 position, string reason)>> failedPositions = new Dictionary<int, List<(Vector3, string)>>();
        // Track area spawn stats
        private Dictionary<int, (int attempted, int succeeded)> areaStats = new Dictionary<int, (int, int)>();

        private void Awake()
        {
            if (mapCenter == null)
            {
                mapCenter = transform;
            }
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnGameplayStateChangeEvent>(OnGameplayStateChanged);
        }

        private void OnGameplayStateChanged(EventData.OnGameplayStateChangeEvent data)
        {
            if (data.GameplayState == Gameplay.GameplayEventType.StartGame && data.ChangeValue)
            {
                StartCoroutine(SpawnInitialBuildings());
            }
            
            
        }

        /// <summary>
        /// Spawns buildings for all areas with a delay between each area
        /// </summary>
        private IEnumerator SpawnInitialBuildings()
        {
            yield return new WaitForSeconds(initialSpawnDelay);

            //Spawn building for each area
            var maxArea = spawnConfig.AreaConfigs.Length;
            for (int areaIndex = 0; areaIndex < maxArea; areaIndex++)
            {
                SpawnBuildingsForArea(areaIndex);
                
                yield return new WaitForSeconds(spawnDelayBetweenAreas);
            }
            
            //Check start active building
            var nearestBuilding = buildingSpawner.GetBuildings(0, amountBuildingActiveInFirstWave, mapCenter.position);
            for (var i = 0; i < amountBuildingActiveInFirstWave; i++)
            {
                nearestBuilding[i].SetActiveBuilding(true, delayTimeActiveTheFirstWave);
            }
        }

        /// <summary>
        /// Spawns buildings for a specific area
        /// </summary>
        private void SpawnBuildingsForArea(int areaIndex)
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
                spawnedBuildingPositions[areaIndex] = new List<Vector3>();
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
                    EnemyBuildingConfig buildingConfig = GetRandomEnemyBuildingConfig(areaConfig, areaIndex);
                    if (buildingConfig == null) 
                    {
                        failedPositions[areaIndex].Add((spawnPosition, "Null building config"));
                        continue;
                    }
                    
                    try
                    {
                        // Create a building data instance
                        var buildingData = new EnemyBuildingData(buildingConfig, areaIndex, areaConfig.BuildingLevel);
                        
                        // Track the position
                        spawnedBuildingPositions[areaIndex].Add(spawnPosition);
                        
                        // Spawn the building
                        buildingSpawner.SpawnBuilding(buildingData, spawnPosition);
                        
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
            Vector3 randomPosition = mapCenter.position + new Vector3(randomDirection.x, 0, randomDirection.y) * distance;
            
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
                foreach (var existingPosition in positions)
                {
                    float distance = Vector3.Distance(position, existingPosition);
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

        [Button]
        private void SaveBuildingPosition()
        {
            var posParent = new GameObject("BuildingPositions").transform;
            foreach (var kvp in spawnedBuildingPositions)
            {
                var posArea = new GameObject($"Area {kvp.Key}").transform;
                posArea.SetParent(posParent);
                for (var i =0; i < kvp.Value.Count; i++)
                {
                    var position = kvp.Value[i];
                    var go = new GameObject($"{kvp.Key} - {position}");
                    go.transform.position = position;
                    go.transform.SetParent(posArea);
                }
            }
        }
        private void OnDrawGizmos()
        {
            if (!drawGizmos || spawnConfig == null || mapCenter == null) return;

            // Draw area ranges
            foreach (var areaConfig in spawnConfig.AreaConfigs)
            {
                // Draw min radius filled circle
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                DrawFilledCircle(mapCenter.position, areaConfig.SpawnRangeMin, 32);

                // Draw max radius filled circle
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                DrawFilledCircle(mapCenter.position, areaConfig.SpawnRangeMax, 32);
            }

            // Draw spawned building positions
            Gizmos.color = Color.blue;
            foreach (var kvp in spawnedBuildingPositions)
            {
                foreach (var position in kvp.Value)
                {
                    Gizmos.DrawSphere(position, debugSphereRadius);
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