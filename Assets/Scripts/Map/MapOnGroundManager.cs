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

        private RewardConstructSpawner rewardConstructSpawner;

        private void Awake()
        {
            rewardConstructSpawner = new();
        }

        public void SpawnObjectOnGround(int areaIndex, UnityAction<List<Vector3>> onCreateConstructSuccess) {
            rewardConstructSpawner.SpawnRewardConstruct(areaIndex, onCreateConstructSuccess);
        }
    }
}
