using System.Collections.Generic;
using UnityEngine;
using Config;

namespace Character.Component {
    public class ScanEnemyComponent
    {
        public List<GameObject> Enemies { get; private set; }

        private readonly CharacterScanConfig config;
        private float scanEnemyTime;

        private ScanEnemyComponent()
        {
            config = Resources.Load<CharacterScanConfig>("CharacterScanConfig");
            Enemies = new();
        }

        public void ScanEnemy(Vector3 characterPos)
        {
            if (scanEnemyTime > Time.time)
            {
                return;
            }

            scanEnemyTime = Time.time + config.ScanEnemyDelay;

            var enemiesAround = Physics2D.OverlapCircleAll(characterPos, config.AttackRange, config.EnemyLayer);

            Enemies.Clear();
            for (var i = 0; i < enemiesAround.Length; i++)
            {
                Enemies.Add(enemiesAround[i].gameObject);
            }
        }
    }
}

