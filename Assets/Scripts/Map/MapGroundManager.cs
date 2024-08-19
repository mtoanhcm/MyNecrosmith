using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map {
    public class MapGroundManager : MonoBehaviour
    {
        public Tilemap GroundMap => groundMap;

        [SerializeField]
        private Tilemap groundMap;
        [SerializeField]
        private TileBase groundTile; // Assign the tile in the inspector

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

        public void SetTilesTypeName(List<Vector3> posLst, string nameType) {
            for (var i = 0; i < posLst.Count; i++) {
                if (IsPositionOnTileMap(posLst[i], out var cellPos) == false) {
                    continue;
                }

                groundMap.GetTile(cellPos).name = nameType;
            }
        }

        private void SetTile(int x, int y)
        {
            Vector3Int tilePosition = new(x, y, 0);
            groundTile.name = EnviromentType.Land.ToString();
            groundMap.SetTile(tilePosition, groundTile);
        }
    }
}

