using System;
using System.Collections.Generic;
using Character;
using Config;
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
            var config = GetEquipmentConfig("Sword", "Sword") as WeaponConfig;
            if (config != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment(){ EquipmentData = new WeaponData(config)});
            }
        }

        private EquipmentConfig GetEquipmentConfig(string equipmentName, string equipmentCategory)
        {
            if (equipmentConfigs.ContainsKey(equipmentName))
            {
                return equipmentConfigs[equipmentName];
            }
            
            var config = Resources.Load<EquipmentConfig>($"Equipment/{equipmentCategory}/{equipmentName}");
            if (config == null)
            {
                Debug.LogError($"Cannot find equipment {equipmentName} config");
                return null;
            }
            
            equipmentConfigs[equipmentName] = config;
            return config;
        }
    }   
}
