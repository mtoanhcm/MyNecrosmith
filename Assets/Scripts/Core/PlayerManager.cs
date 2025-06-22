using System;
using System.Threading.Tasks;
using Building;
using Config;
using Equipment;
using GameUtility;
using NUnit.Framework.Constraints;
using Observer;
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

        [SerializeField] private BuildingBase minionCastle;
        
        private PlayerInventory inventory;

        protected override void Awake()
        {
            base.Awake();
            
            TryGetComponent(out inventory);
            inventory.Init();
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
            EventManager.Instance.StartListening<EventData.OnRemoveEquipmentFromPlayerStorage>(OnRemoveEquipment);

            InitStartupEquipment();
            InitCastleBase();
        }

        private async Task InitCastleBase()
        {
            if (minionCastle == null)
            {
                return;
            }
            
            var config = await AddressableUtility.LoadAssetAsync<MinionBuildingConfig>("Config/Building/MinionCastleConfig.asset");
            if (config == null)
            {
                Debug.LogError($"Cannot get minion castle config");
                return;
            }

            minionCastle.Spawn(Vector3.zero, new BuildingData(config, 0, 1));
            minionCastle.SetActiveBuilding(true);
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
            EventManager.Instance.TriggerEvent(new EventData.OnPlayerInventoryChanged { HasChange = true});
        }

        private async Task InitStartupEquipment()
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

                    inventory.AddEquipmentToStorage(config.Group switch
                    {
                        EquipmentGroup.Weapon => new WeaponData(config),
                        EquipmentGroup.Armor => new ArmorData(config),
                        _ => null
                    });
                }
            }
        }
    }   
}
