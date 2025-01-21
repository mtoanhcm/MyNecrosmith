using System.Collections.Generic;
using Config;
using Observer;
using UnityEngine;
using Equipment;

namespace Spawner
 {
     public class EquipmentSpawner : MonoBehaviour
     {
         public List<EquipmentBase> Equipments => equipments;
        
         private List<EquipmentBase> equipments;

         private void OnLoadPrefabSuccess(EventData.OnLoadEquipmentPrefabSuccess data)
         {
             // if (prefabDictionary.ContainsKey(data.EquipmentTypeID))
             // {
             //     return;
             // }
             //
             // prefabDictionary.Add(data.EquipmentTypeID, data.EquipmentPrefab);
         }

         private async void PrepareSwordPrefab()
         {
             var equipmentBase = await ResourcesManager.Instance.LoadEquipmentPrefabAsync(WeaponID.Sword.ToString());
             var sword = equipmentBase as IronSword;
             if (sword == null)
             {
                 Debug.LogError($"Cannot load {WeaponID.Sword} prefab");
                 return;
             }
            
             //prefabDictionary.Add(WeaponID.Sword.ToString(), sword);
         }
         
         private void SpawnEquipment(EventData.OnSpawnEquipment data)
         {
             // var equipment = objectPool.GetObject(data.Equipment.ID);
             // if (equipment == null)
             // {
             //     Debug.LogError($"Cannot instantiate equipment {data.Equipment.ID}");
             //     return;
             // }
             //
             // equipment.Init(data.Owner, data.Equipment, data.SpawnPosition);
             // data.OnSpawnEqupimentSuccessHandle?.Invoke(equipment);
         }
     }
 }
