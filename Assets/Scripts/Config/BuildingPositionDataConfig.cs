using Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BuildingPositionDataConfig", menuName = "baseConfig/Building/BuildingPositionDataConfig")]
    public class BuildingPositionDataConfig : ScriptableObject
    {
        [Serializable]
        public struct BuildingPositionData
        {
            public int AreaIndex;
            public Vector3 Position;
            public Rarity Rarity;
            public int Level;
        }

        [SerializeField]
        private BuildingPositionData[] buildingPositionData;

        public void SetBuildingPositionData(Dictionary<int, List<Vector3>> data)
        {
            List<BuildingPositionData> buildingPositionDataList = new List<BuildingPositionData>();
            foreach (var key in data) { 
                for(var i =0; i < key.Value.Count; i++)
                {
                    BuildingPositionData buildingPositionData = new BuildingPositionData();
                    buildingPositionData.AreaIndex = key.Key;
                    buildingPositionData.Position = key.Value[i];
                    buildingPositionData.Rarity = Rarity.Common;
                    buildingPositionData.Level = 1;

                    buildingPositionDataList.Add(buildingPositionData);
                }
            }

            buildingPositionData = buildingPositionDataList.ToArray();
        }
    }
}
