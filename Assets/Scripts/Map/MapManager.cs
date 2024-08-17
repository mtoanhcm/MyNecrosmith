using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map {
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        public MapGroundManager groundManager;
        public FogManager fogManager;
        public MapOnGroundManager onGroundManager;

        private MapConfig config;

        private void Awake()
        {
            if (Instance == null) { 
                Instance = this;
            }

            config = Resources.Load<MapConfig>("MapConfig");
        }

        private void Start()
        {
            var areaRadius = config.GetAreaByIndex(1).Radius;
            groundManager.CreateTilemap(areaRadius);
            fogManager.CreateFogMap(areaRadius);
            onGroundManager.SpawnObjectOnGround(1, UpdateClaimGround);
        }

        public void OnCheckClearFog(Vector3 basePosition, float clearRadius) {
            fogManager.OpenFog(basePosition, clearRadius);
        }

        public bool IsValidPointOnMap(Vector3 point) {
            return groundManager.IsPositionOnTileMap(point);
        }

        private void UpdateClaimGround(List<Vector3> claimedPosList) { 
        
        }
    }
}
