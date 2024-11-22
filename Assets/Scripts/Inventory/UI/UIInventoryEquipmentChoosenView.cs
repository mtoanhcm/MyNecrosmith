using System;
using Config;
using Equipment;
using Observer;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIInventoryEquipmentChoosenView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI equipmentNameTxt;
        [SerializeField] private TextMeshProUGUI effectAttributeTxt;
        [SerializeField] private TextMeshProUGUI equipmentTypeTxt;
        [SerializeField] private TextMeshProUGUI loadPointTxt;
        
        [SerializeField] private UIInventoryItem equipmentUIItemPrefab;
        [SerializeField] private Transform equipmentUIItemContainer;
        
        
        private void OnEnable()
        {
            EventManager.Instance?.StartListening<EventData.OnChooseEquipmentInStorage>(OnChooseEquipmentInStorage);
        }

        private void OnDisable()
        {
            EventManager.Instance?.StopListening<EventData.OnChooseEquipmentInStorage>(OnChooseEquipmentInStorage);
        }

        private void OnChooseEquipmentInStorage(EventData.OnChooseEquipmentInStorage data)
        {
            if (data.Equipment == null)
            {
                ResetInfo();
                return;
            }
            
            equipmentNameTxt.text = data.Equipment.Name;
            effectAttributeTxt.text = data.Equipment.EffectValue;
            equipmentTypeTxt.text = data.Equipment.EffectType;
            loadPointTxt.text = $"Load point: {data.Equipment.LoadPoint.ToString()}";
            
            var equipmentUIItem = Instantiate(equipmentUIItemPrefab, equipmentUIItemContainer);
            equipmentUIItem.Init(data.Equipment);
        }

        private void ResetInfo()
        {
            equipmentNameTxt.text = string.Empty;
            effectAttributeTxt.text = string.Empty;
            equipmentTypeTxt.text = string.Empty;
            loadPointTxt.text = string.Empty;
        }
    }   
}
