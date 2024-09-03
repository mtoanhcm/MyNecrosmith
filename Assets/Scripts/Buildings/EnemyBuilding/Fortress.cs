using Character;
using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public class Fortress : BuildingBase
    {
        private CharacterBase enemyPrefab;
        private CharacterConfig config;

        public override void Claimp()
        {
            
        }

        public override void PlayActivation()
        {
            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
            config.TryGetCharacterData(CharacterID.SimpleEnemy, out var data);
            enemy.InitComponent(CharacterID.SimpleEnemy, data);
        }

        public override void OnAwake()
        {
            delayActiveTime = 2f;
            enemyPrefab = Resources.Load<CharacterBase>("SimpleEnemy");
            config = Resources.Load<CharacterConfig>("CharacterConfig");
        }

        public override void TakeDamage(float damage)
        {
            
        }
    }
}
