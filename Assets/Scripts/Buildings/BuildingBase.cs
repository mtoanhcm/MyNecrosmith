using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public abstract class BuildingBase : MonoBehaviour
    {
        protected BuildingData data;
        protected bool isActive;
        
        protected float delayActiveTime;
        private float tempDelayActiveTime;

        public void Init(Vector3 pos ,BuildingData initData) { 
            data = initData;
        }

        public abstract void Spawn();
        public abstract void Claim();
        public abstract void TakeDamage(float damage);
        public abstract void PlayActivation();

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
    }
}
