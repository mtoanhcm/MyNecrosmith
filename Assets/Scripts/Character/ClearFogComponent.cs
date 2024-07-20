using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ClearFogComponent : MonoBehaviour
{
    [SerializeField]
    private FogManager fogManager;
    [SerializeField]
    private float visibleRadius;

    private Vector3Int previousTilePosition;

    private void Start()
    {
        // Initialize the previousTilePosition to the character's starting tile position
        CheckClearFog();
        Debug.Log("Done check fog");
    }

    private void Update()
    {
        if (fogManager.ConverWorldPosToTileMapPos(transform.position) == previousTilePosition) {
            return;
        }

        CheckClearFog();
    }

    private void CheckClearFog() {
        previousTilePosition = fogManager.ConverWorldPosToTileMapPos(transform.position);
        fogManager.OpenFog(transform.position, visibleRadius);
    }
}
