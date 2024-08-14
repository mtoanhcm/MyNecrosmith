using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map {
    public class FogManager : MonoBehaviour
    {
        [SerializeField]
        private Tilemap fogMap;
        [SerializeField]
        private TileBase fogTile;

        public void CreateFogMap(int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector3Int tilePosition = new(x, y, 0);
                    SetFog(tilePosition);
                }
            }
        }

        public void OpenFog(Vector3 position, float radius)
        {
            Vector3Int characterPos = fogMap.WorldToCell(position);
            int radiusInCells = Mathf.CeilToInt(radius);

            for (int x = -radiusInCells; x <= radiusInCells; x++)
            {
                for (int y = -radiusInCells; y <= radiusInCells; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(characterPos.x + x, characterPos.y + y, characterPos.z);
                    Vector3Int offset = tilePosition - characterPos;

                    if (offset.x * offset.x + offset.y * offset.y <= radius * radius)
                    {
                        SetFog(tilePosition, false);
                    }
                }
            }
        }

        public bool IsPositionClear(Vector3 position) {
            Vector3Int cellPosition = fogMap.WorldToCell(position);
            TileBase tile = fogMap.GetTile(cellPosition);
            return tile == null;
        }

        public Bounds GetClearAreaBounds()
        {
            Bounds bounds = new Bounds();
            foreach (var position in fogMap.cellBounds.allPositionsWithin)
            {
                if (fogMap.GetTile(position) == null)
                {
                    Vector3 worldPosition = fogMap.CellToWorld(position);
                    bounds.Encapsulate(worldPosition);
                }
            }

            // Thu hẹp bounds 3 ô từ mỗi cạnh
            float tileWidth = fogMap.cellSize.x;
            float tileHeight = fogMap.cellSize.y;

            Vector3 min = bounds.min + new Vector3(tileWidth * 5, tileHeight * 5, 0);
            Vector3 max = bounds.max - new Vector3(tileWidth * 5, tileHeight * 5, 0);

            bounds.SetMinMax(min, max);

            return bounds;
        }

        private void SetFog(Vector3Int pos, bool isVisibleFog = true)
        {
            fogMap.SetTile(pos, isVisibleFog ? fogTile : null);
        }
    }
}
