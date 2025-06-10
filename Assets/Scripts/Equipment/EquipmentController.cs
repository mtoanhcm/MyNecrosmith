using System.Collections.Generic;
using System.Text;
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

        private void Update()
        {
            if (equipments.Count == 0)
            {
                return;
            }

            StickEquipmentToPlayer();
        }

        public void Attack(Transform target)
        {
            for (var i = 0; i < equipments.Count; i++)
            {
                var equipment = equipments[i];

                if(equipment.Data.Group != Config.EquipmentGroup.Weapon)
                {
                    continue;
                }

                equipments[i].PerformAction(target);
            }
        }
        
        public void AddEquipment(EquipmentData[] equipmentData, CharacterBase owner)
        {
            equipmentPositions = transform.GetEquipmentPositionAroundCharacter(equipmentData.Length);
            for (var i = 0; i < equipmentData.Length; i++)
            {
                var equipment = equipmentData[i];
                
                if(equipment is WeaponData)
                {
                    SpawnWeaponEquipment(equipment, equipmentPositions[i]);
                    continue;
                }
                
                if(equipment is ArmorData armorData)
                {
                    AddArmorEquipmentEffect(armorData);
                    continue;
                }
            }

            return;

            void AddArmorEquipmentEffect(ArmorData armorData)
            {
                owner.CharacterHealth.UpdateMaxHealth(armorData.HP);
                
                if(owner is MinionCharacter minion)
                {
                    minion.MinionData.SetArmorType(armorData.ArmorType);
                }
            }

            void SpawnWeaponEquipment(EquipmentData equipment, Vector3 spawnPos)
            {
                var data = new EventData.OnSpawnEquipment()
                {
                    EquipmentID = equipment.EquipmentID.ToString(),
                    EquipmentCategoryID = equipment.CategoryID.ToString(),
                    OnSpawnEquipmentSuccessHandle = OnSpawnEquipmentSuccess
                };

                EventManager.Instance.TriggerEvent(data);

                return;

                void OnSpawnEquipmentSuccess(EquipmentBase equipmentBase)
                {
                    equipmentBase.Init(owner, equipment, spawnPos);
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
                equipments[i].transform.position = transform.position + transform.rotation * equipmentPositions[i];
            }
        }
    }   
}
