using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Observer;
using static Observer.EventData;
using Config;
using Tile;

namespace Map {
    public class MapGroundManager : MonoBehaviour
    {
        public Tilemap GroundMap => groundMap;

        [SerializeField]
        private Tilemap groundMap;

        private TileConfig tileConfig;

        private void Awake()
        {
            tileConfig = Resources.Load<TileConfig>("TileConfig");
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<CLaimGroundTile>(SetClaimType);
        }

        public void CreateTilemap(int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    SetTile(x, y);
                }
            }
        }

        public bool IsPositionOnTileMap(Vector3 pos, out Vector3Int cellPos) { 
            cellPos = groundMap.WorldToCell(pos);
            return groundMap.HasTile(cellPos);
        }

        private void SetClaimType(CLaimGroundTile data) {
            for (var i = 0; i < data.ClaimPos.Count; i++) {
                if (IsPositionOnTileMap(data.ClaimPos[i], out var cellPos) == false) {
                    continue;
                }

                groundMap.SetTile(cellPos, tileConfig.GetTile(TileType.Blocker));
            }
        }

        private void SetTile(int x, int y)
        {
            Vector3Int tilePosition = new(x, y, 0);
            groundMap.SetTile(tilePosition, tileConfig.GetTile(TileType.GrassGround));
        }
    }
}

