using Character;
using Config;
using Pool;
using UnityEngine;

namespace Building {
    public class Fortress : BuildingBase
    {
        private CharacterBase enemyPrefab;
        private CharacterConfig config;

        private float delayStartActiveTime;
        private StatData characterSpawnData;

        public override void Claimp()
        {
            
        }

        public override void PlayActivation()
        {
            var enemy = CharacterPoolManager.Instance.SpawnEnemy();
            enemy.Spawn(CharacterID.SimpleEnemy, transform.position, characterSpawnData);
        }

        public override void OnSubInit()
        {
            delayActiveTime = 5;
            //enemyPrefab = Resources.Load<CharacterBase>("SimpleEnemy");
            config = Resources.Load<CharacterConfig>("CharacterConfig");
            config.TryGetCharacterData(CharacterID.SimpleEnemy, out characterSpawnData);

            delayStartActiveTime = Time.time + data.TimeToStartActivation;

            StartCoroutine(ProgressActivation());
        }

        public override void TakeDamage(float damage)
        {
            
        }

        public override void Explose()
        {
           
        }

        public override bool CanActive()
        {
            return delayStartActiveTime < Time.time;
        }
    }
}
