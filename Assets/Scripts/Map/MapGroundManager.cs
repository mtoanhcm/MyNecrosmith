using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGroundManager : MonoBehaviour
{
    [SerializeField]
    private Grid mapGrid;
    [SerializeField]
    private Tilemap groundMap;
    [SerializeField]
    private TileBase groundTile; // Assign the tile in the inspector

    [SerializeField]
    private int radius;

    [Header("----- Component -----")]
    [SerializeField]
    private FogManager fogManage;

    private void Start()
    {
        CreateTileMap();
        Debug.Log("Done create tile map");
    }

    [Button]
    private void CreateTileMap()
    {
        // Set the size and tiles
        groundMap.ClearAllTiles();
        CreateTilemap(radius); // Example size of 10x10
    }

    private void CreateTilemap(int radius) {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                SetTile(x, y);
            }
        }
    }

    private void SetTile(int x, int y) {
        Vector3Int tilePosition = new (x, y, 0);
        groundMap.SetTile(tilePosition, groundTile);
        fogManage.SetFog(tilePosition);
    }
}
