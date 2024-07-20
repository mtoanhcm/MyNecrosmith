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

        private void SetFog(Vector3Int pos, bool isVisibleFog = true)
        {
            fogMap.SetTile(pos, isVisibleFog ? fogTile : null);
        }
    }
}
