using System.Collections;
using UnityEngine;

namespace Building {
    
    [RequireComponent(typeof(BuildingHealth))]
    public abstract class BuildingBase : MonoBehaviour
    {
        private BuildingData data;
        protected bool isActive;
        protected BuildingHealth buildingHealth;
        
        protected float activationCooldownTime;
        private float tempActivationCooldownTime;

        public BuildingID ID => data.ID;
        public int AreaIndex => data.AreaIndex;
        public int Level => data.Level;
        public BuildingHealth Health => buildingHealth;
        public bool IsActive => isActive;
        
        public virtual void Spawn(Vector3 pos, BuildingData initData)
        {
             data = initData;
             transform.position = pos;

             InitHealth();
        }

        public void SetActiveBuilding(bool hasActive, int delayActiveTime)
        {
            StopAllCoroutines();
            
            if (delayActiveTime > 0)
            {
                StartCoroutine(ProgressActiveBuildingByTime(hasActive ,delayActiveTime));
                return;
            }
            
            isActive = hasActive;
        }

        public void SetActiveBuilding(bool hasActive)
        {
            StopAllCoroutines();
            isActive = hasActive;
        }

        private IEnumerator ProgressActiveBuildingByTime(bool hasActive ,float seconds)
        {
            yield return new WaitForSeconds(seconds);

            isActive = hasActive;
        }
        
        protected abstract void PlayActivation();

        protected abstract void OnBuildingClaimed();

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            if (tempActivationCooldownTime > Time.time) {
                return;
            }

            tempActivationCooldownTime = Time.time + activationCooldownTime;
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
