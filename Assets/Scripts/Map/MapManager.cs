using Config;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [Button]
        private void TestShowTileMap()
        {
            foreach (Vector3Int position in groundManager.GroundMap.cellBounds.allPositionsWithin) {
                if (groundManager.GroundMap.HasTile(position)) {
                    var tile = groundManager.GroundMap.GetTile(position);
                    Debug.Log($"Tile at pos {position} name: {tile.name}");
                }
            }
        }

        public bool IsValidPointOnMap(Vector3 point) {
            return groundManager.IsPositionOnTileMap(point, out _);
        }
    }
}
