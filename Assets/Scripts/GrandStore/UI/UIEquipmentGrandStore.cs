using System;
using System.Collections.Generic;
using Config;
using Gameplay;
using GameUtility;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIEquipmentGrandStore : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem equipmentItemPrefab;
        [SerializeField] private UIEquipmentSpawner equipmentUIItemSpawner;
        [SerializeField] private Transform itemContainer;
        
        private List<UIInventoryItem> inventoryItems;
        private GameRuntimeData runtimeData;

        private void Awake()
        {
            inventoryItems = new();
            equipmentUIItemSpawner.Init();
            runtimeData = Resources.Load<GameRuntimeData>("GameRuntimeData");
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);

            ShowEqupimentList();
        }

        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
        }

        private void Start()
        {
            
        }

        private void ShowEqupimentList()
        {
            var equipmentLst = runtimeData.EquipmentStorage;
            if (equipmentLst == null || equipmentLst.Count == 0)
            {
                return;
            }
            
            for (var i = 0; i < equipmentLst.Count; i++)
            {
                if (inventoryItems.Count <= i)
                {
                    var newItem = Instantiate(equipmentItemPrefab, itemContainer);
                    newItem.Init(equipmentLst[i]);
                    
                    inventoryItems.Add(newItem);
                    continue;
                }
                
                inventoryItems[i].Init(equipmentLst[i]);
            }
        }

        private void GetEquipmentFromStorage(UIEquipmentStoreItem item)
        {
            var uiEquipmentDrag = equipmentUIItemSpawner.GetEquipmentUIItem();
            uiEquipmentDrag.Init(item.EquipmentData);
            uiEquipmentDrag.transform.SetParent(itemContainer);
            
            uiEquipmentDrag.ActiveDragging();
            
            //inventoryItems.Remove(item);
            item.SetActive(false);
        }
        
        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            var newEquipmentItem = Instantiate(equipmentItemPrefab, itemContainer);
            newEquipmentItem.Init(data.EquipmentData);
            
            inventoryItems.Add(newEquipmentItem);
        }
    }   
}
