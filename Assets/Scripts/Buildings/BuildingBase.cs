using Observer;
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
        protected bool isVisibleOnMap;

        private float tempDelayAcTiveTime;

        public void Init(int id, Vector3 pos ,BuildingData initData) { 
            data = initData;
            ID = id;
            postition = pos;
        }

        public void ChangeVisibleOnMap(bool isVisible)
        {
            isVisibleOnMap = isVisible;
        }

        public abstract void OnAwake();
        public abstract void Claimp();
        public abstract void TakeDamage(float damage);
        /// <summary>
        /// Building activity in <paramref name="delayActiveTime"/> period
        /// </summary>
        public abstract void PlayActivation();

        private void Awake()
        {
            OnAwake();
        }

        private void Update()
        {
            //Building only works when visible on map
            if (!isVisibleOnMap) {
                return;
            }

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
