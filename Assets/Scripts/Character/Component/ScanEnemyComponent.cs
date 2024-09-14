using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Character.Component {
    public class ScanEnemyComponent
    {
        public List<CharacterBase> Enemies { get; private set; }

        private readonly float scanRadius;
        private readonly float scanDelayTime;
        private readonly float scanEnemyTime;
        private readonly int enemyLayer;

        private bool canScan;

        public ScanEnemyComponent(float radius, int enemyLayer, float delay = 1f)
        {
            Enemies = new List<CharacterBase>();
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

        // ReSharper disable Unity.PerformanceAnalysis
        private async void ScanProgress(Transform characterScan)
        {
            var delayTime = (int)(scanDelayTime * 1000);
            var enemiesAround = new Collider2D[100];
            while (canScan) {
                _ = Physics2D.OverlapCircleNonAlloc(characterScan.position, scanRadius, enemiesAround, enemyLayer);

                Enemies.Clear();
                for (var i = 0; i < enemiesAround.Length; i++)
                {
                    if (enemiesAround[i] == null)
                    {
                        continue;
                    }
                    
                    if (enemiesAround[i].gameObject.TryGetComponent<CharacterBase>(out var enemy) && enemy.IsAlive)
                    {
                        Enemies.Add(enemy);
                    }
                }
                
                await UniTask.Delay(delayTime);
            }
        }
    }
}

