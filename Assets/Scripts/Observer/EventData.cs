using Tile;
using System.Collections.Generic;
using UnityEngine;

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
        /// Set all ground tile in list by the target Tile Type
        /// </summary>
        public struct CLaimGroundTile {
            public List<Vector3> ClaimPos;
            public TileType TileType;
        }
    }
}
