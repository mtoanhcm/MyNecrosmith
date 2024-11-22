using System.Collections.Generic;
using Config;
using Equipment;
using Observer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class PlayerManager : MonoBehaviour
    {
        private GameRuntimeData runtimeData;
        
        private void Awake()
        {
            runtimeData = Resources.Load<GameRuntimeData>("GameRuntimeData");
            EventManager.Instance.StartListening<EventData.OnObtainedEquipment>(OnObtainedEquipment);
        }

        private void OnObtainedEquipment(EventData.OnObtainedEquipment data)
        {
            runtimeData.AddEquipmentToStorage(data.EquipmentData);
        }

        [Button]
        public void TestPickupSword()
        {
            var swordConfig = Resources.Load<EquipmentConfig>("Equipment/Weapon/SwordConfig");
            if (swordConfig != null)
            {
                EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment(){ EquipmentData = new WeaponData(swordConfig) });
            }
        }
    }   
}
