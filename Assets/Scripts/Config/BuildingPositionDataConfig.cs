using Gameplay;
using Newtonsoft.Json;
using SaveLoad;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BuildingPositionDataConfig", menuName = "baseConfig/Building/BuildingPositionDataConfig")]
    public class BuildingPositionDataConfig : ScriptableObject
    {
        [Serializable]
        public class BuildingPositionData
        {
            public int AreaIndex;
            public Rarity Rarity;
            public float PosX;
            public float PosY;
            public float PosZ;

            [JsonIgnore]
            public Vector3 Position => new Vector3(PosX, PosY, PosZ);
        }

        [SerializeField]
        private BuildingPositionData[] buildingPositionData;

        public void SetBuildingPositionData(List<BuildingPositionData> dataPos)
        {
            buildingPositionData = dataPos.ToArray();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public BuildingPositionData[] GetBuildingPositionData()
        {
            return buildingPositionData;
        }
    }
}
