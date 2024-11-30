using System.Collections.Generic;
using GameUtility;
using Observer;
using UnityEngine;
using Equipment;

namespace Spawner
 {
     public class EquipmentSpawner : ObjectSpawner<EquipmentBase>
     {
         protected override void Start()
         {
             base.Start();
             EventManager.Instance.StartListening<EventData.OnSpawnEquipment>(SpawnEquipment);
             EventManager.Instance.StartListening<EventData.OnLoadEquipmentPrefabSuccess>(OnLoadPrefabSuccess);
         }

         private void OnLoadPrefabSuccess(EventData.OnLoadEquipmentPrefabSuccess data)
         {
             if (prefabDictionary.ContainsKey(data.EquipmentTypeID))
             {
                 return;
             }
             
             prefabDictionary.Add(data.EquipmentTypeID, data.EquipmentPrefab);
         }

         private void SpawnEquipment(EventData.OnSpawnEquipment data)
         {
             var equipment = objectPool.GetObject(data.Equipment.ID);
             if (equipment == null)
             {
                 Debug.LogError($"Cannot instantiate equipment {data.Equipment.ID}");
                 return;
             }

             equipment.Init(data.Owner, data.Equipment, data.SpawnPosition);
             data.OnSpawnEqupimentSuccessHandle?.Invoke(equipment);
         }
     }
 }
