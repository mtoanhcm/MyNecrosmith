using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Observer;
using Config;
using Tile;
using Unity.VisualScripting;
using UnityEngine.Profiling;

namespace Map {
    public class FogManager : MonoBehaviour
    {
        public bool IsInit { get; private set; }
        public Tilemap FogMap => fogMap;

        [SerializeField]
        private Tilemap fogMap;
        [SerializeField]
        private TileBase fogTile;

        private TileConfig tileConfig;

        private const float DELAY_CHECK_VISIBLE_BOUND = 1f;

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
            bool isOpenNewFogCell = false;
            int radiusInCells = Mathf.CeilToInt(data.Radius);

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
                        isOpenNewFogCell = true;
                    }
                }
            }

            if (isOpenNewFogCell)
            {
                EventManager.Instance.TriggerEvent(new EventData.OpenFogWarSuccessEvent() { IsOpenFogSuccess = true });
            }
        }

        public bool IsPositionClear(Vector3 position) {
            Vector3Int cellPosition = fogMap.WorldToCell(position);
            TileBase tile = fogMap.GetTile(cellPosition);
            return tile == null;
        }

        private void SetFog(Vector3Int pos, bool isVisibleFog = true)
        {
            fogMap.SetTile(pos, isVisibleFog ? tileConfig.GetTile(TileType.Fog) : null);
        }
    }
}
