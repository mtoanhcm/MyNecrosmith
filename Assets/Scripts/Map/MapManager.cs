using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map {
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        public MapGroundManager groundManager;
        public FogManager fogManager;

        private MapConfig config;

        private void Awake()
        {
            if (Instance == null) { 
                Instance = this;
            }

            config = Resources.Load<MapConfig>("MapConfig");
        }

        private void Start()
        {
            groundManager.CreateTilemap(config.FirstArea.Radius);
            fogManager.CreateFogMap(config.FirstArea.Radius);
        }

        public void OnCheckClearFog(Vector3 basePosition, float clearRadius) {
            fogManager.OpenFog(basePosition, clearRadius);
        }
    }
}
