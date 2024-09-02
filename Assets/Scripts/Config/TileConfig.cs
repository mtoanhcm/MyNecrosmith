using Tile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

namespace Config {
    [CreateAssetMenu(fileName = "TileConfig", menuName = "Config/TileConfig", order = 1)]
    public class TileConfig : ScriptableObject
    {
        [Serializable]
        public struct TileData { 
            public TileType TileType;
            public TileBase Tile;
        }

        [SerializeField]
        private List<TileData> Tiles;

        public TileBase GetTile(TileType tileType) {
            for (var i = 0; i < Tiles.Count; i++) { 
                if(Tiles[i].TileType == tileType)
                {
                    return Tiles[i].Tile;
                }
            }

            return null;
        }

        public TileType GetTileType(TileBase tile) {
            for (var i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].Tile.Equals(tile))
                {
                    return Tiles[i].TileType;
                }
            }

            return TileType.None;
        }
    }
}
