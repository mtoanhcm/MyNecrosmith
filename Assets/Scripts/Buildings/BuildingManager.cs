using System.Collections;
using System.Collections.Generic;
using Config;
using Observer;
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
        [SerializeField] private int maxAreaIndex = 3;
        [SerializeField] private LayerMask buildingLayer;
        
        [Header("Debug")]
        [SerializeField] private bool drawGizmos = false;
        [SerializeField] private bool verboseLogging = true;
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
            
            LogInfo($"BuildingManager initialized with maxAreaIndex: {maxAreaIndex}");
        }

        private void Start()
        {
            LogInfo("Starting initial building spawn sequence");
            StartCoroutine(SpawnInitialBuildings());
        }

        /// <summary>
        /// Spawns buildings for all areas with a delay between each area
        /// </summary>
        private IEnumerator SpawnInitialBuildings()
        {
            LogInfo($"Waiting {initialSpawnDelay} seconds before spawning");
            yield return new WaitForSeconds(initialSpawnDelay);

            LogInfo($"Beginning area spawning sequence (0 to {maxAreaIndex - 1})");
            for (int areaIndex = 0; areaIndex < maxAreaIndex; areaIndex++)
            {
                LogInfo($"Spawning buildings for area {areaIndex}");
                SpawnBuildingsForArea(areaIndex);
                
                if (areaStats.TryGetValue(areaIndex, out var stats)) 
                {
                    LogInfo($"Area {areaIndex} spawn stats: {stats.succeeded}/{stats.attempted} successful ({(stats.attempted > 0 ? stats.succeeded * 100f / stats.attempted : 0):F1}%)");
                }
                
                LogInfo($"Waiting {spawnDelayBetweenAreas} seconds before next area");
                yield return new WaitForSeconds(spawnDelayBetweenAreas);
            }
            
            LogInfo("Finished spawning buildings for all areas");
            LogSpawnSummary();
        }

        /// <summary>
        /// Spawns buildings for a specific area
        /// </summary>
        public void SpawnBuildingsForArea(int areaIndex)
        {
            LogInfo($"SpawnBuildingsForArea: Starting for area {areaIndex}");
            
            var areaConfig = spawnConfig.GetAreaConfig(areaIndex);
            if (areaConfig == null)
            {
                LogError($"Cannot spawn buildings - area config for index {areaIndex} not found");
                return;
            }

            LogInfo($"Area {areaIndex} config: SpawnRangeMin={areaConfig.SpawnRangeMin}, SpawnRangeMax={areaConfig.SpawnRangeMax}, " +
                   $"BuildingAmount={areaConfig.BuildingAmount}, BuildingDistance={areaConfig.BuildingDistance}");
            
            if (areaConfig.EnemyBuildingConfigs == null)
            {
                LogError($"Area {areaIndex}: EnemyBuildingConfigs is null");
                return;
            }
            
            LogInfo($"Area {areaIndex}: EnemyBuildingConfigs count = {areaConfig.EnemyBuildingConfigs.Length}");
            
            // Initialize positions tracker for this area if needed
            if (!spawnedBuildingPositions.ContainsKey(areaIndex))
            {
                spawnedBuildingPositions[areaIndex] = new List<Vector3>();
                LogInfo($"Initialized position tracking for area {areaIndex}");
            }
            
            // Initialize failed positions tracker
            if (!failedPositions.ContainsKey(areaIndex))
            {
                failedPositions[areaIndex] = new List<(Vector3, string)>();
            }

            int attemptedBuildings = 0;
            int successfullySpawned = 0;
            int maxAttempts = areaConfig.BuildingAmount * 10; // Limit attempts to avoid infinite loop

            LogInfo($"Area {areaIndex}: Starting spawn loop. Target: {areaConfig.BuildingAmount} buildings");
            
            while (successfullySpawned < areaConfig.BuildingAmount && attemptedBuildings < maxAttempts)
            {
                attemptedBuildings++;
                
                if (attemptedBuildings % 10 == 0)
                {
                    LogInfo($"Area {areaIndex}: Attempted {attemptedBuildings} spawns, succeeded {successfullySpawned} so far");
                }
                
                // Get random position within the area's range
                Vector3 spawnPosition = GetRandomPositionInArea(areaConfig, areaIndex);
                LogDebug($"Area {areaIndex}: Generated position at {spawnPosition}");
                
                // Check if position is valid for building placement
                string failReason = "";
                if (IsValidBuildingPosition(spawnPosition, areaIndex, areaConfig.BuildingDistance, out failReason))
                {
                    LogDebug($"Area {areaIndex}: Position {spawnPosition} is valid");
                    
                    // Get random enemy building config from available options
                    EnemyBuildingConfig buildingConfig = GetRandomEnemyBuildingConfig(areaConfig, areaIndex);
                    if (buildingConfig == null) 
                    {
                        LogError($"Area {areaIndex}: Failed to get valid EnemyBuildingConfig");
                        failedPositions[areaIndex].Add((spawnPosition, "Null building config"));
                        continue;
                    }

                    LogDebug($"Area {areaIndex}: Selected building config ID={buildingConfig.ID}");
                    
                    try
                    {
                        // Create a building data instance
                        var buildingData = new EnemyBuildingData(buildingConfig);
                        
                        // Track the position
                        spawnedBuildingPositions[areaIndex].Add(spawnPosition);
                        LogInfo($"Area {areaIndex}: Added position {spawnPosition} to tracking list");
                        
                        // Spawn the building
                        LogInfo($"Area {areaIndex}: Calling buildingSpawner.SpawnBuilding with {buildingConfig.ID} at {spawnPosition}");
                        buildingSpawner.SpawnBuilding(buildingData, spawnPosition);
                        
                        successfullySpawned++;
                        LogInfo($"Area {areaIndex}: Successfully spawned building {successfullySpawned}/{areaConfig.BuildingAmount}");
                    }
                    catch (System.Exception e)
                    {
                        LogError($"Area {areaIndex}: Exception during building spawn: {e.Message}\n{e.StackTrace}");
                        failedPositions[areaIndex].Add((spawnPosition, $"Exception: {e.Message}"));
                    }
                }
                else
                {
                    LogDebug($"Area {areaIndex}: Position {spawnPosition} is invalid: {failReason}");
                    failedPositions[areaIndex].Add((spawnPosition, failReason));
                }
            }

            // Store spawn stats for this area
            areaStats[areaIndex] = (attemptedBuildings, successfullySpawned);

            if (successfullySpawned < areaConfig.BuildingAmount)
            {
                LogWarning($"Area {areaIndex}: Could only spawn {successfullySpawned}/{areaConfig.BuildingAmount} buildings after {attemptedBuildings} attempts");
                if (failedPositions[areaIndex].Count > 0)
                {
                    LogWarning($"Area {areaIndex}: Top failure reasons:");
                    Dictionary<string, int> reasonCounts = new Dictionary<string, int>();
                    foreach (var (_, reason) in failedPositions[areaIndex])
                    {
                        if (!reasonCounts.ContainsKey(reason))
                            reasonCounts[reason] = 0;
                        reasonCounts[reason]++;
                    }
                    
                    foreach (var kvp in reasonCounts)
                    {
                        LogWarning($"  - {kvp.Key}: {kvp.Value} times");
                    }
                }
            }
            else
            {
                LogInfo($"Area {areaIndex}: Successfully spawned all {successfullySpawned} buildings after {attemptedBuildings} attempts");
            }
        }

        /// <summary>
        /// Spawns buildings for a specific area with a delay
        /// </summary>
        public void SpawnBuildingsForAreaDelayed(int areaIndex, float delay)
        {
            LogInfo($"Scheduling delayed spawn for area {areaIndex} in {delay} seconds");
            StartCoroutine(SpawnAreaDelayed(areaIndex, delay));
        }

        private IEnumerator SpawnAreaDelayed(int areaIndex, float delay)
        {
            yield return new WaitForSeconds(delay);
            SpawnBuildingsForArea(areaIndex);
        }

        /// <summary>
        /// Clear buildings tracking data for a specific area
        /// Note: This doesn't despawn actual buildings, just clears the position tracking
        /// </summary>
        public void ClearBuildingsInArea(int areaIndex)
        {
            if (spawnedBuildingPositions.ContainsKey(areaIndex))
            {
                LogInfo($"Clearing position tracking for area {areaIndex}, had {spawnedBuildingPositions[areaIndex].Count} positions");
                spawnedBuildingPositions[areaIndex].Clear();
            }
            
            if (failedPositions.ContainsKey(areaIndex))
            {
                failedPositions[areaIndex].Clear();
            }
        }

        /// <summary>
        /// Gets a random position within the area's range
        /// </summary>
        private Vector3 GetRandomPositionInArea(AreaBuildingConfig areaConfig, int areaIndex)
        {
            // Get random direction
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            
            // Get random distance within range
            float distance = Random.Range(areaConfig.SpawnRangeMin, areaConfig.SpawnRangeMax);
            
            // Calculate position
            Vector3 randomPosition = mapCenter.position + new Vector3(randomDirection.x, 0, randomDirection.y) * distance;
            LogDebug($"Area {areaIndex}: Generated raw position at {randomPosition}, direction={randomDirection}, distance={distance}");
            
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
        private EnemyBuildingConfig GetRandomEnemyBuildingConfig(AreaBuildingConfig areaConfig, int areaIndex)
        {
            if (areaConfig.EnemyBuildingConfigs == null || areaConfig.EnemyBuildingConfigs.Length == 0)
            {
                LogError($"Area {areaIndex}: No enemy building configs found");
                return null;
            }
            
            int randomIndex = Random.Range(0, areaConfig.EnemyBuildingConfigs.Length);
            var config = areaConfig.EnemyBuildingConfigs[randomIndex];
            
            if (config == null)
            {
                LogError($"Area {areaIndex}: Enemy building config at index {randomIndex} is null");
                return null;
            }
            
            return config;
        }
        
        /// <summary>
        /// Logs a summary of the spawn results for all areas
        /// </summary>
        private void LogSpawnSummary()
        {
            LogInfo("========== BUILDING SPAWN SUMMARY ==========");
            
            int totalAttempted = 0;
            int totalSucceeded = 0;
            int totalTargeted = 0;
            
            foreach (var areaConfig in spawnConfig.AreaConfigs)
            {
                int areaIndex = areaConfig.AreaIndex;
                int targeted = areaConfig.BuildingAmount;
                totalTargeted += targeted;
                
                string statInfo = "No spawn attempt made";
                if (areaStats.TryGetValue(areaIndex, out var stats))
                {
                    totalAttempted += stats.attempted;
                    totalSucceeded += stats.succeeded;
                    
                    float successRate = stats.attempted > 0 ? (float)stats.succeeded / stats.attempted * 100 : 0;
                    statInfo = $"{stats.succeeded}/{targeted} buildings ({successRate:F1}% success rate)";
                }
                
                LogInfo($"Area {areaIndex}: {statInfo}");
            }
            
            float overallRate = totalAttempted > 0 ? (float)totalSucceeded / totalAttempted * 100 : 0;
            float completionRate = totalTargeted > 0 ? (float)totalSucceeded / totalTargeted * 100 : 0;
            
            LogInfo($"Overall: {totalSucceeded}/{totalTargeted} buildings spawned");
            LogInfo($"Attempt success rate: {overallRate:F1}%, Completion rate: {completionRate:F1}%");
            LogInfo("=============================================");
        }

        #region Logging Methods
        private void LogInfo(string message)
        {
            if (verboseLogging)
            {
                Debug.Log($"[BuildingManager] {message}");
            }
        }

        private void LogDebug(string message)
        {
            if (verboseLogging)
            {
                Debug.Log($"[BuildingManager][Debug] {message}");
            }
        }

        private void LogWarning(string message)
        {
            Debug.LogWarning($"[BuildingManager] {message}");
        }

        private void LogError(string message)
        {
            Debug.LogError($"[BuildingManager] {message}");
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!drawGizmos || spawnConfig == null || mapCenter == null) return;
            
            // Draw area ranges
            foreach (var areaConfig in spawnConfig.AreaConfigs)
            {
                // Draw min radius
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                DrawCircle(mapCenter.position, areaConfig.SpawnRangeMin, 32);
                
                // Draw max radius
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                DrawCircle(mapCenter.position, areaConfig.SpawnRangeMax, 32);
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

        private void DrawCircle(Vector3 center, float radius, int segments)
        {
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