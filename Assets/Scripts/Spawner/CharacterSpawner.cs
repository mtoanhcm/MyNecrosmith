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
    public class CharacterSpawner : ObjectSpawner<CharacterBase>
    {
        private Dictionary<string, MinionCharacter> characterPrefabDic;
        
        protected override void Start()
        {
            base.Start();
            
            EventManager.Instance.StartListening<EventData.OnSpawnMinion>(SpawnMinion);
        }

        protected override CharacterBase CreateObjectForPool(string characterID)
        {
            if (characterPrefabDic.TryGetValue(characterID, out var characterPrefab))
            {
                return Instantiate(characterPrefab, transform);
            }

            return null;
        }

        private async void PrepareMinionPrefab(CharacterID id)
        {
            var characterObj = await AddressableUtility.LoadAssetAsync<GameObject>($"Assets/Characters/{id}");
            if (characterObj == null)
            {
                Debug.LogError($"Cannot load character base");
                return;
            }
            
            // minionCharacter = characterObj.GetComponent<MinionCharacter>();
            // if (minionCharacter == null)
            // {
            //     Debug.LogError($"Cannot parse character base to minion character");
            // }
        }

        private void SpawnMinion(EventData.OnSpawnMinion data)
        {
            // var minion = objectPool.GetObject("Minion") as MinionCharacter;
            // if (minion == null)
            // {
            //     Debug.LogError("Cannot instantiate minion character");
            //     return;
            // }
            //
            // minion.Spawn(new MinionData(data.Config));
            // minion.InitEquipment(data.Equipments);
            // minion.transform.position = data.SpawnPosition;
        }
    }
}
