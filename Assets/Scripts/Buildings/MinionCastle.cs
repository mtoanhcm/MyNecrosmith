using Config;
using Equipment;
using Observer;
using Spawner;
using UnityEngine;

namespace Building
{
    public class MinionCastle : BuildingBase
    {
        [SerializeField] private MinionSpawner spawner;
        
        //Only for quick test
        [SerializeField] private EquipmentConfig swordConfig;

        public override void Spawn(Vector3 pos, BuildingData initData)
        {
            base.Spawn(pos, initData);
            activationCooldownTime = 5f;
        }

        // Update is called once per frame
        protected override void PlayActivation()
        {
            //Create equipment runtime
            EventManager.Instance.TriggerEvent(new EventData.OnObtainedEquipment()
            {
                EquipmentData = new WeaponData(swordConfig)
            });
        }

        protected override void OnBuildingClaimed()
        {
            //Show gameover
            Destroy(gameObject);
        }
    }   
}
