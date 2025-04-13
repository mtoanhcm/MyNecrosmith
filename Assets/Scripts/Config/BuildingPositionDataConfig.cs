using Gameplay;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BuildingPositionDataConfig", menuName = "baseConfig/Building/BuildingPositionDataConfig")]
    public class BuildingPositionDataConfig : ScriptableObject
    {
        public class BuildingPositionData : MonoBehaviour
        {
            public int AreaIndex;
            public Vector3 Position;
            public Rarity Rarity;
        }

        [SerializeField]
        private BuildingPositionData[] buildingPositionData;

        public void SetBuildingPositionData(Dictionary<int, List<Transform>> data)
        {
            List<BuildingPositionData> buildingPositionDataList = new List<BuildingPositionData>();
            foreach (var key in data) { 
                for(var i =0; i < key.Value.Count; i++)
                {
                    var posData = key.Value[i].GetOrAddComponent<BuildingPositionData>();
                    posData.AreaIndex = key.Key;
                    posData.Position = key.Value[i].position;
                    posData.Rarity = Rarity.Common;

                    buildingPositionDataList.Add(posData);
                }
            }

            buildingPositionData = buildingPositionDataList.ToArray();
        }
    }
}
