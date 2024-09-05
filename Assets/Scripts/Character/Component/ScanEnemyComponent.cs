using System.Collections.Generic;
using UnityEngine;
using Config;
using Cysharp.Threading.Tasks;
using System;

namespace Character.Component {
    public class ScanEnemyComponent
    {
        public List<CharacterBase> Enemies { get; private set; }

        private float scanRadius;
        private float scanDelayTime;
        private float scanEnemyTime;
        private int enemyLayer;

        private bool canScan;

        public ScanEnemyComponent(float radius, int enemyLayer, float delay = 1f)
        {
            Enemies = new();
            scanRadius = radius;
            scanDelayTime = delay;
            this.enemyLayer = enemyLayer;
        }

        public void StartScan(Transform characterTrans) { 
            canScan = true;
            ScanProgress(characterTrans);
        }

        public void StopScan() {
            canScan = false;
        }

        private async void ScanProgress(Transform characterScan)
        {
            int delayTime = (int)(scanDelayTime * 1000);
            while (canScan) {

                await UniTask.Delay(delayTime);
                var enemiesAround = Physics2D.OverlapCircleAll(characterScan.position, scanRadius, enemyLayer);

                Enemies.Clear();
                for (var i = 0; i < enemiesAround.Length; i++)
                {
                    if (enemiesAround[i].gameObject.TryGetComponent<CharacterBase>(out var enemy))
                    {
                        Enemies.Add(enemy);
                    }
                }
            }
        }
    }
}

