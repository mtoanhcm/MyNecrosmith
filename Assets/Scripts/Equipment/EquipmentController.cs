using System;
using System.Collections.Generic;
using Character;
using GameUtility;
using Observer;
using UnityEngine;

namespace Equipment
{
    public class EquipmentController : MonoBehaviour
    {
        public List<EquipmentBase> Equipments => equipments;
        
        private List<EquipmentBase> equipments;
        private List<Vector3> equipmentPositions;
        private void Awake()
        {
            equipments = new List<EquipmentBase>();
        }

        private void LateUpdate()
        {
            if (equipments.Count == 0)
            {
                return;
            }
            
            StickEquipmentToPlayer();
        }

        public void AddEquipment(List<EquipmentData> equipmentData, CharacterBase owner)
        {
            equipmentPositions = transform.position.GetEquipmentPositionAroundCharacter(equipmentData.Count);
            for (var i = 0; i < equipmentData.Count; i++)
            {
                var equipment = equipmentData[i];
                var equipmentPosition = equipmentPositions[i];
                
                var data = new EventData.OnSpawnEquipment()
                {
                    EquipmentID = equipment.ID,
                    EquipmentCategoryID = equipment.CategoryID,
                    OnSpawnEquipmentSuccessHandle = OnSpawnEquipmentSuccess
                };
                
                EventManager.Instance.TriggerEvent(data);

                void OnSpawnEquipmentSuccess(EquipmentBase equipmentBase)
                {
                    equipmentBase.Init(owner, equipment, equipmentPosition);
                    equipments.Add(equipmentBase);
                }
            }
        }

        public void ReleaseAllEquipment()
        {
            for (var i = 0; i < equipments.Count; i++)
            {
                var equipment = equipments[i];
                
                var data = new EventData.OnDestroyEquipment()
                {
                    Equipment = equipment
                };
                
                EventManager.Instance.TriggerEvent(data);
            }
            
            equipments.Clear();
        }

        private void StickEquipmentToPlayer()
        {
            for (var i = 0; i < equipments.Count; i++)
            {
                equipments[i].transform.position = transform.position + equipmentPositions[i];
            }
        }
    }   
}
