using Building;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Component
{
    public class ScanBuildingComponent
    {
        public List<Building.BuildingBase> Buildings { get; private set; }

        private float scanRadius;
        private float scanDelayTime;
        private float scanEnemyTime;
        private int buildingLayer;

        private bool canScan;

        public ScanBuildingComponent(float radius, int buildingLayer, float delay = 1f) {
            Buildings = new();
            scanRadius = radius;
            scanDelayTime = delay;
            this.buildingLayer = buildingLayer;
        }

        public void StartScan(Transform characterTrans)
        {
            canScan = true;
            ScanProgress(characterTrans);
        }

        public void StopScan()
        {
            canScan = false;
        }

        private async void ScanProgress(Transform characterScan)
        {
            int delayTime = (int)(scanDelayTime * 1000);
            while (canScan)
            {

                await UniTask.Delay(delayTime);
                var buildingAround = Physics2D.OverlapCircleAll(characterScan.position, scanRadius, buildingLayer);

                Buildings.Clear();
                for (var i = 0; i < buildingAround.Length; i++)
                {
                    if (buildingAround[i].gameObject.TryGetComponent<BuildingBase>(out var building))
                    {
                        Buildings.Add(building);
                    }
                }
            }
        }
    }
}
