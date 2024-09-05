using Tile;
using System.Collections.Generic;
using UnityEngine;
using Building;

namespace Observer {
    public class EventData
    {
        /// <summary>
        /// Open fog of war by radius
        /// </summary>
        public struct OpenFogOfWarEvent
        {
            public Vector3 Pos;
            public float Radius;
        }
        
        /// <summary>
        /// Report the open fog action success or not
        /// </summary>
        public struct OpenFogWarSuccessEvent {
            public bool IsOpenFogSuccess;
            public List<Vector3Int> OpenFogCells;
        }

        /// <summary>
        /// Set all ground tile in list by the target Tile Type
        /// </summary>
        public struct CLaimGroundTile {
            public List<Vector3> ClaimPos;
            public TileType TileType;
        }
    }
}
