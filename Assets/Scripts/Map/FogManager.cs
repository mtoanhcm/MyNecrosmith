using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Observer;
using Config;
using Tile;

namespace Map {
    public class FogManager : MonoBehaviour
    {
        public bool IsInit { get; private set; }

        [SerializeField]
        private Tilemap fogMap;
        [SerializeField]
        private TileBase fogTile;

        private TileConfig tileConfig;

        private void Awake()
        {
            tileConfig = Resources.Load<TileConfig>("TileConfig");
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OpenFogOfWarEvent>(OpenFog);
        }

        public void InitFogMap(int radius)
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

        private void OpenFog(EventData.OpenFogOfWarEvent data)
        {
            Vector3Int characterPos = fogMap.WorldToCell(data.Pos);
            int radiusInCells = Mathf.CeilToInt(data.Radius);
            bool isClearFogSuccess = false;

            for (int x = -radiusInCells; x <= radiusInCells; x++)
            {
                for (int y = -radiusInCells; y <= radiusInCells; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(characterPos.x + x, characterPos.y + y, characterPos.z);
                    if (fogMap.HasTile(tilePosition) == false) {
                        continue;
                    }

                    Vector3Int offset = tilePosition - characterPos;

                    if (offset.x * offset.x + offset.y * offset.y <= data.Radius * data.Radius)
                    {
                        SetFog(tilePosition, false);
                        isClearFogSuccess = true;
                    }
                }
            }

            if (isClearFogSuccess) {
                EventManager.Instance.TriggerEvent(new EventData.OpenFogWarSuccessEvent() { IsOpenFogSuccess = isClearFogSuccess });
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

            if (bounds.min == Vector3.zero && bounds.max == Vector3.zero) {
                return bounds;
            }

            float tileWidth = fogMap.cellSize.x;
            float tileHeight = fogMap.cellSize.y;

            // Thu hẹp bounds 5 ô từ mỗi cạnh
            Vector3 min = bounds.min + new Vector3(tileWidth * 5, tileHeight * 5, 0);
            Vector3 max = bounds.max - new Vector3(tileWidth * 5, tileHeight * 5, 0);

            bounds.SetMinMax(min, max);

            return bounds;
        }

        private void SetFog(Vector3Int pos, bool isVisibleFog = true)
        {
            fogMap.SetTile(pos, isVisibleFog ? tileConfig.GetTile(TileType.Fog) : null);
        }
    }
}
