using System;
using System.Collections.Generic;
using Config;
using GameUtility;
using Observer;
using UnityEngine;
using Character;
using UnityEngine.AddressableAssets;

namespace Spawner
{
    public class CharacterSpawner : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnPrepareEquipmentForSpawnMinion>(SpawnMinion);
        }

        private void SpawnMinion(EventData.OnPrepareEquipmentForSpawnMinion data)
        {
            var spawnData = new EventData.OnSpawnMinion()
            {
                MinionID = data.MinionConfig.ID.ToString(),
                OnSpawnSuccess = OnSpawnMinionSuccess
            };

            EventManager.Instance.TriggerEvent(spawnData);
            
            return;

            void OnSpawnMinionSuccess(CharacterBase character)
            {
                var minion = character as MinionCharacter;
                if(minion == null)
                {
                    Debug.LogError($"Cannot spawn minion {data.MinionConfig.ID}");
                    return;
                }
                
                minion.Spawn(new MinionData(data.MinionConfig));
                minion.InitEquipment(data.Equipment);
                minion.transform.position = transform.position;
            }
        }
    }
}
