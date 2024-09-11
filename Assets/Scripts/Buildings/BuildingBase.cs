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
        private bool isInit;

        public void Init(int id, Vector3 pos ,BuildingData initData) { 
            data = initData;
            ID = id;
            postition = pos;

            OnSubInit();

            isInit = true;
        }

        public void ChangeVisibleOnMap(bool isVisible)
        {
            isVisibleOnMap = isVisible;
        }
        /// <summary>
        /// Call for addition Init
        /// </summary>
        public virtual void OnSubInit() { }
        public virtual void OnAwake() { }
        /// <summary>
        /// Call this action to claim this building
        /// </summary>
        public abstract void Claimp();
        /// <summary>
        /// Apply damage to building
        /// </summary>
        /// <param name="damage">Damage amount</param>
        public virtual void TakeDamage(float damage) {
            data.HP -= Mathf.Max(0, damage);
            if (data.HP <= 0) {
                Explose();
            }
        }
        /// <summary>
        /// Call to destroy this building
        /// </summary>
        public abstract void Explose();
        /// <summary>
        /// Building activity in <paramref name="delayActiveTime"/> period
        /// </summary>
        public abstract void PlayActivation();
        /// <summary>
        /// Condition for the activition of the building
        /// </summary>
        /// <returns>True: can active, False: Nope</returns>
        public virtual bool CanActive() {
            return false;
        }

        private void Awake()
        {
            isInit = false;

            OnAwake();
        }

        protected IEnumerator ProgressActivation() {
            while (true) {

                if (CanActive() == false) {
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                
                PlayActivation();
                yield return new WaitForSeconds(delayActiveTime);
            }
        }

        //private void Update()
        //{
        //    if (!isInit) {
        //        return;
        //    }

        //    if (!CanActive()) {
        //        return;
        //    }

        //    if (tempDelayAcTiveTime > Time.time) {
        //        return;
        //    }

        //    tempDelayAcTiveTime = Time.time + delayActiveTime;
        //    PlayActivation();
        //}
    }
}
