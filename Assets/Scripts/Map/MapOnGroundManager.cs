using Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Map {
    public class MapOnGroundManager : MonoBehaviour
    {
        [SerializeField]
        private Tilemap onGroundTileMap;

        private ConstructSpawner constructSpawner;

        private void Awake()
        {
            constructSpawner = new();
        }

        public void SpawnObjectOnGround(int areaIndex) {
            constructSpawner.SpawnConstruct(areaIndex);
        }
    }
}
