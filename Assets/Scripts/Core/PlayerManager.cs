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
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerManager : SingletonForScene<PlayerManager>
    {
        [Serializable]
        public struct EquipmentInitData
        {
            public EquipmentID EquipmentID;
            public EquipmentCategoryID Category;
            public int Amount;
        }

        public PlayerInventory Inventory => inventory;
        
        [SerializeField] 
        private EquipmentInitData[] initEquipment;
        
        private PlayerInventory inventory;
        
        private void Awake()
        {
            TryGetComponent(out inventory);
            inventory.Init();
            
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
            EventManager.Instance.StartListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipment);
        }

        private void Start()
        {
            InitStartupEquipment();
        }

        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            inventory.AddEquipmentToStorage(data.EquipmentData);
            SendEventEquipmentStorageChanged();
        }

        private void OnRemoveEquipment(EventData.OnRemoveEquipmentFromPlayerStorage data)
        {
            inventory.RemoveEquipment(data.EquipmentData);
            SendEventEquipmentStorageChanged();
        }

        private void SendEventEquipmentStorageChanged()
        {
            
        }

        private async void InitStartupEquipment()
        {
            for (var i = 0; i < initEquipment.Length; i++)
            {
                var equipment = initEquipment[i];
                for (var j = 0; j < equipment.Amount; j++)
                {
                    var path = $"Config/Equipment/{equipment.Category}/{equipment.EquipmentID}.asset";
                    var config = await AddressableUtility.LoadAssetAsync<EquipmentConfig>(path);
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
                inventory.AddEquipmentToStorage(new WeaponData(config));
                return true;
            }

            return false;
        }
    }   
}
