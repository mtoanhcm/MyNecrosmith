using System;
using System.Collections.Generic;
using Config;
using GameUtility;
using Observer;
using UnityEngine;
using Character;

namespace Spawner
{
    public class CharacterSpawner : ObjectSpawner<CharacterBase>
    {
        protected override void Start()
        {
            base.Start();

            PrepareMinionPrefab();
            
            EventManager.Instance.StartListening<EventData.OnSpawnMinion>(SpawnMinion);
            EventManager.Instance.StartListening<EventData.OnLoadCharacterPrefabSuccess>(OnLoadPrefabSuccess);
        }
        
        private async void PrepareMinionPrefab()
        {
            var characterBase = await ResourcesManager.Instance.LoadCharacterPrefabAsync("Minion", CharacterID.HumanKnight.ToString());
            var MinionCharacter = characterBase as MinionCharacter;
            if (MinionCharacter == null)
            {
                Debug.LogError($"Cannot load {CharacterID.HumanKnight} prefab");
                return;
            }
            
            prefabDictionary.Add(CharacterID.HumanKnight.ToString(), MinionCharacter);
        }

        private void OnLoadPrefabSuccess(EventData.OnLoadCharacterPrefabSuccess data)
        {
            if (prefabDictionary.ContainsKey(data.Class))
            {
                return;
            }
            
            prefabDictionary.Add(data.Class, data.CharPrefab);
        }

        private void SpawnMinion(EventData.OnSpawnMinion data)
        {
            var minion = objectPool.GetObject(data.Config.ID.ToString()) as MinionCharacter;
            if (minion == null)
            {
                Debug.LogError("Cannot instantiate minion character");
                return;
            }

            minion.Spawn(new MinionData(data.Config));
            minion.InitEquipment(data.Equipments);
            minion.transform.position = data.SpawnPosition;
        }
    }
}
