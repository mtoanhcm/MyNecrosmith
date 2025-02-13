using System;
using System.Collections.Generic;
using Config;
using Equipment;
using GameUtility;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class PlayerManager : MonoBehaviour
    {
        [Serializable]
        public struct EquipmentInitData
        {
            public EquipmentID EquipmentID;
            public EquipmentCategoryID Category;
            public int Amount;
        }

        [SerializeField] 
        private EquipmentInitData[] initEquipment;
        
        private GameRuntimeData runtimeData;
        
        private void Awake()
        {
            runtimeData = Resources.Load<GameRuntimeData>("GameRuntimeData");
            #if UNITY_EDITOR
            if (runtimeData != null)
            {
                runtimeData.Reset();
            }
            #endif
            
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
            EventManager.Instance.StartListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipment);
        }

        private void Start()
        {
            InitStartupEquipment();
        }

        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            runtimeData.AddEquipmentToStorage(data.EquipmentData);
            SendEventEquipmentStorageChanged();
        }

        private void OnRemoveEquipment(EventData.OnRemoveEquipmentFromPlayerStorage data)
        {
            runtimeData.RemoveEquipmentFromStorage(data.EquipmentID);
            SendEventEquipmentStorageChanged();
        }

        private void SendEventEquipmentStorageChanged()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnEquipmentStorageChanged()
            {
                Equipment = runtimeData.EquipmentStorage
            });
        }

        private void InitStartupEquipment()
        {
            for (var i = 0; i < initEquipment.Length; i++)
            {
                var equipment = initEquipment[i];
                for (var j = 0; j < equipment.Amount; j++)
                {
                    var config = Resources.Load($"Equipment/{equipment.Category}/{equipment.EquipmentID}") as EquipmentConfig;
                    if (config == null)
                    {
                        Debug.LogError($"Could not load equipment {equipment.EquipmentID} from Resources");
                        continue;
                    }

                    if (AddWeaponEquipment(config))
                    {
                        continue;
                    }
                }
            }
        }

        private bool AddWeaponEquipment(EquipmentConfig config)
        {
            if (config.CategoryID.IsWeaponType())
            {
                runtimeData.AddEquipmentToStorage(new WeaponData(config));
                return true;
            }

            return false;
        }
    }   
}
