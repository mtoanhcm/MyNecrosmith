using System.Collections.Generic;
using UnityEngine;
using Config;

namespace Character.Component {
    public class ScanEnemyComponent
    {
        public List<GameObject> Enemies { get; private set; }

        private float scanRadius;
        private float scanDelayTime;
        private float scanEnemyTime;
        private int enemyLayer;

        private ScanEnemyComponent(float radius, int enemyLayer, float delay = 1f)
        {
            Enemies = new();
            scanRadius = radius;
            scanDelayTime = delay;
            this.enemyLayer = enemyLayer;
        }

        public void ScanEnemy(Vector3 characterPos)
        {
            if (scanEnemyTime > Time.time)
            {
                return;
            }

            scanEnemyTime = Time.time + scanDelayTime;

            var enemiesAround = Physics2D.OverlapCircleAll(characterPos, scanRadius, enemyLayer);

            Enemies.Clear();
            for (var i = 0; i < enemiesAround.Length; i++)
            {
                Enemies.Add(enemiesAround[i].gameObject);
            }
        }
    }
}

