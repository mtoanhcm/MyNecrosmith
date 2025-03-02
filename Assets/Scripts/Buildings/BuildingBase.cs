using UnityEngine;

namespace Building {
    
    [RequireComponent(typeof(BuildingHealth))]
    public abstract class BuildingBase : MonoBehaviour
    {
        protected BuildingData data;
        protected bool isActive;
        protected BuildingHealth buildingHealth;
        protected float delayActiveTime;
        private float tempDelayActiveTime;

        public BuildingID BuildingID => data.ID;
        
        public virtual void Spawn(Vector3 pos, BuildingData initData)
        {
             data = initData;
             transform.position = pos;

             InitHealth();
        }
        
        protected abstract void PlayActivation();

        protected abstract void OnBuildingClaimed();

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            if (tempDelayActiveTime > Time.time) {
                return;
            }

            tempDelayActiveTime = Time.deltaTime + delayActiveTime;
            PlayActivation();
        }

        private void InitHealth()
        {
            if (buildingHealth == null)
            {
                TryGetComponent(out buildingHealth);
            }
            
            buildingHealth.Init(data, OnBuildingClaimed);
        }
    }
}
