using System;
using Config;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class UIEquipmentGrandStore : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem inventoryItemPrefab;

        private EquipmentConfig config;

        private void Awake()
        {
            config = Resources.Load<EquipmentConfig>("Equipment/Sword/IronSword");
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnPickingEquipmentFromInventory>(OnPickingFromInventory);
        }

        private void OnPickingFromInventory(EventData.OnPickingEquipmentFromInventory data)
        {
            data.UIItemPick.transform.SetParent(transform);
        }

        [Button]
        private void TestSpawnItem()
        {
            var swordData = new Equipment.EquipmentData(config);
            
            var swordObj = Instantiate(inventoryItemPrefab, transform);
            swordObj.Init(swordData);
            
            swordObj.transform.localPosition = Vector3.zero;
        }
    }   
}
