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
        [SerializeField] private UIEquipmentStoreItem equipmentItemPrefab;
        [SerializeField] private UIEquipmentSpawner equipmentUIItemSpawner;
        [SerializeField] private Transform itemContainer;
        
        private List<UIEquipmentStoreItem> inventoryItems;
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
            EventManager.Instance.StartListening<EventData.OnPickingEquipmentFromInventory>(OnPickingFromInventory);
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
                    newItem.SetData(equipmentLst[i], GetEquipmentFromStorage);
                    
                    inventoryItems.Add(newItem);
                    continue;
                }
                
                inventoryItems[i].SetData(equipmentLst[i], GetEquipmentFromStorage);
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
            newEquipmentItem.SetData(data.EquipmentData, GetEquipmentFromStorage);
            
            inventoryItems.Add(newEquipmentItem);
        }
        
        private void OnPickingFromInventory(EventData.OnPickingEquipmentFromInventory data)
        {
            data.UIItemPick.transform.SetParent(itemContainer);
        }
    }   
}
