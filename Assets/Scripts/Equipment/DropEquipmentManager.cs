using System;
using System.Collections.Generic;
using Character;
using Config;
using GameUtility;
using Observer;
using UnityEngine;

namespace Equipment.Drop
{
    public class DropEquipmentManager : MonoBehaviour
    {
        private Dictionary<string, EquipmentConfig> equipmentConfigs;

        private void Awake()
        {
            equipmentConfigs = new Dictionary<string, EquipmentConfig>();
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnEnemyDeath>(OnEnemyDeath);
        }

        private void OnEnemyDeath(EventData.OnEnemyDeath data)
        {
            SpawnSword();
        }

        private async void SpawnSword()
        {
            EquipmentConfig config = null;
            if (equipmentConfigs.ContainsKey("Sword"))
            {
                config = equipmentConfigs["Sword"] as WeaponConfig;
            }
            else
            {
                var path = $"Config/Equipment/Sword/Sword.asset";
                config = await AddressableUtility.LoadAssetAsync<EquipmentConfig>(path);
                if (config == null)
                {
                    equipmentConfigs["Sword"] = config;   
                }
            }
            
            if (config != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment(){ EquipmentData = new WeaponData(config)});
            }
        }
    }   
}
