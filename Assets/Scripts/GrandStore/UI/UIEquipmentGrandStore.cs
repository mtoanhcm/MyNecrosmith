using System.Collections.Generic;
using Equipment;
using Gameplay;
using Observer;
using UnityEngine;

namespace UI
{
    public class UIEquipmentGrandStore : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem equipmentItemPrefab;
        [SerializeField] private UIEquipmentSpawner equipmentUIItemSpawner;
        [SerializeField] private Transform cellContainer;
        
        private List<UIEquipmentStorageCell> equipmentCells;
        private List<EquipmentData> equipmentDatas;
        private GameRuntimeData runtimeData;

        private void Awake()
        {
            equipmentCells = new List<UIEquipmentStorageCell>(cellContainer.GetComponentsInChildren<UIEquipmentStorageCell>());
            equipmentDatas = new List<EquipmentData>();
            
            equipmentUIItemSpawner.Init();
            runtimeData = Resources.Load<GameRuntimeData>("GameRuntimeData");
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);

            ShowEquipmentList();
        }

        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
        }

        private void ShowEquipmentList()
        {
            var equipmentLst = runtimeData.EquipmentStorage;
            if (equipmentLst == null || equipmentLst.Count == 0)
            {
                return;
            }
            
            equipmentDatas.Clear();
            for (var i = 0; i < equipmentLst.Count; i++)
            {
                AddEquipmentToStorage(equipmentLst[i]);
            }
        }

        private void AddEquipmentToStorage(EquipmentData data)
        {
            equipmentDatas.Add(data);

            UpdateEquipmentUI(data);
        }

        private void UpdateEquipmentUI(EquipmentData data)
        {
            for (var i = 0; i < equipmentCells.Count; i++)
            {
                var cell = equipmentCells[i];
                if (cell.HasEquipment)
                {
                    continue;
                }
                
                cell.SetData(data);
                break;
            }
        }
        
        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            AddEquipmentToStorage(data.EquipmentData);
        }
    }   
}
