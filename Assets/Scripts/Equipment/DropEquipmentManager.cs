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
        [SerializeField]
        private EquipmentDropRateConfig config;

        private void Awake()
        {
            config.Init();
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnEnemyDeath>(OnEnemyDeath);
        }

        private void OnEnemyDeath(EventData.OnEnemyDeath data)
        {
            if (config == null) {
                Debug.LogError($"Cannt drop equipment because of nullable config");
                return;
            }

            if(config.GetRandomEquipmentID(out var equipmentConfig))
            {
                EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment() { EquipmentData = new WeaponData(equipmentConfig) });
            }
        }
    }   
}
