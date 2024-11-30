using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public abstract class BuildingBase : MonoBehaviour
    {
        public Vector3 Position => postition;

        protected int ID;
        protected Vector3 postition;
        protected BuildingData data;

        protected float delayActiveTime;
        private float tempDelayAcTiveTime;

        public void Init(int id, Vector3 pos ,BuildingData initData) { 
            data = initData;
            ID = id;
            postition = pos;
        }

        public abstract void Spawn();
        public abstract void Claimp();
        public abstract void TakeDamage(float damage);
        public abstract void PlayActivation();

        private void Update()
        {
            if (delayActiveTime <= 0) {
                return;
            }

            if (tempDelayAcTiveTime > Time.time) {
                return;
            }

            tempDelayAcTiveTime = Time.deltaTime + delayActiveTime;
            PlayActivation();
        }
    }
}
