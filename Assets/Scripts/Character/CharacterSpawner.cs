using System;
using System.Collections.Generic;
using Config;
using Equipment;
using GameUtility;
using Observer;
using UnityEngine;

namespace Character
{
    public class CharacterSpawner : MonoBehaviour
    {
        private GameObjectPool<CharacterBase> characterPool;
        private Dictionary<string, CharacterBase> characterPrefabDic = new Dictionary<string, CharacterBase>();
        
        public void Start()
        {
            characterPool = new GameObjectPool<CharacterBase>(CreateCharacterForPool, OnGetCharacterFromPool, OnReturnCharacterToPool, transform);
            
            EventManager.Instance.StartListening<EventData.OnSpawnMinion>(SpawnMinion);
        }

        private void SpawnMinion(EventData.OnSpawnMinion data)
        {
            var minion = characterPool.GetObject(data.Config.Class.ToString()) as MinionCharacter;
            if (minion == null)
            {
                Debug.LogError("Cannot instantiate minion character");
                return;
            }
            
            minion.Spawn(new CharacterStats(data.Config));
            minion.InitEquipment(data.Equipments);
            minion.transform.position = data.SpawnPosition;
        }
        
        public void SpawnEnemy(CharacterConfig config, Vector3 spawnPosition)
        {
            
        }
        
        private CharacterBase CreateCharacterForPool(string characterType)
        {
            if (!characterPrefabDic.TryGetValue(characterType, out var characterPrefab))
            {
                Debug.LogError($"Character {characterType} resources cannot prepare");
                return null;
            }
            
            return Instantiate(characterPrefab, transform);
        }
        
        private void OnReturnCharacterToPool(CharacterBase obj)
        {
            throw new NotImplementedException();
        }

        private void OnGetCharacterFromPool(CharacterBase obj)
        {
            throw new NotImplementedException();
        }
        
        private void OnDestroyCharacter(CharacterBase obj)
        {
            Destroy(obj.gameObject);
        }
    }   
}
