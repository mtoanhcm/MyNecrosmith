using System;
using System.Collections.Generic;
using Config;
using Gameplay;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class UIEquipmentGrandStore : MonoBehaviour
    {
        [SerializeField] private UIEquipmentStoreItem inventoryItemPrefab;
        [SerializeField] private Transform itemContainer;
        
        private List<UIEquipmentStoreItem> inventoryItems;
        private GameRuntimeData runtimeData;

        private void Awake()
        {
            inventoryItems = new();
            runtimeData = Resources.Load<GameRuntimeData>("GameRuntimeData");
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);

            ShowEqupimentList();
        }

        private void OnDisable()
        {
            EventManager.Instance.StopListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
        }

        private void Start()
        {
            EventManager.Instance.StartListening<EventData.OnPickingEquipmentFromInventory>(OnPickingFromInventory);
        }

        private void ShowEqupimentList()
        {
            var equipmentLst = runtimeData.EquipmentStorage;
            for (var i = 0; i < equipmentLst.Count; i++)
            {
                if (inventoryItems.Count <= i)
                {
                    var newItem = Instantiate(inventoryItemPrefab, itemContainer);
                    newItem.SetData(equipmentLst[i]);
                    
                    inventoryItems.Add(newItem);
                    continue;
                }
                
                inventoryItems[i].SetData(equipmentLst[i]);
            }
        }

        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            var newEquipmentItem = Instantiate(inventoryItemPrefab, itemContainer);
            newEquipmentItem.SetData(data.EquipmentData);
        }
        
        private void OnPickingFromInventory(EventData.OnPickingEquipmentFromInventory data)
        {
            data.UIItemPick.transform.SetParent(itemContainer);
        }
    }   
}
