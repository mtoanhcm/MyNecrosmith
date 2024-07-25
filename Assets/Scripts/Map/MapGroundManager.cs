using Sirenix.OdinInspector;
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

        private void SetTile(int x, int y)
        {
            Vector3Int tilePosition = new(x, y, 0);
            groundMap.SetTile(tilePosition, groundTile);
        }
    }
}

