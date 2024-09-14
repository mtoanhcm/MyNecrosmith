using Config;
using Pathfinding;
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
            fogManager.InitFogMap(areaRadius);
            onGroundManager.SpawnObjectOnGround(1);
        }

        public bool IsValidPointOnMap(Vector3 point) {
            return groundManager.IsPositionOnTileMap(point, out _);
        }
    }
}
