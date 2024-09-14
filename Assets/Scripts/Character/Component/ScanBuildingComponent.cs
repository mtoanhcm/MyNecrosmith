using Building;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Component
{
    public class ScanBuildingComponent
    {
        public List<BuildingBase> Buildings { get; private set; }

        private readonly float scanRadius;
        private readonly float scanDelayTime;
        private readonly float scanEnemyTime;
        private readonly int buildingLayer;

        private bool canScan;

        public ScanBuildingComponent(float radius, int buildingLayer, float delay = 1f) {
            Buildings = new List<BuildingBase>();
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

        // ReSharper disable Unity.PerformanceAnalysis
        private async void ScanProgress(Transform characterScan)
        {
            var delayTime = (int)(scanDelayTime * 1000);
            var buildingAround = new Collider2D[8];
            while (canScan)
            {
                _ = Physics2D.OverlapCircleNonAlloc(characterScan.position, scanRadius, buildingAround, buildingLayer);

                Buildings.Clear();
                for (var i = 0; i < buildingAround.Length; i++)
                {
                    if (buildingAround[i] == null)
                    {
                        continue;
                    }
                    
                    if (buildingAround[i].gameObject.TryGetComponent<BuildingBase>(out var building))
                    {
                        Buildings.Add(building);
                    }
                }
                
                await UniTask.Delay(delayTime);
            }
        }
    }
}
