using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Character {
    public abstract class CharacterBase : MonoBehaviour
    {
        

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void OnDestroy()
        {
            Destroy();
        }

        private void Update()
        {
            OnUpdateExcute();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void Destroy() { }

        public virtual void MoveToTarget(Vector3 targetPos, UnityAction onEndPath) { }
        public virtual void Attack(GameObject target) { }

        /// <summary>
        /// Init character component, need to call when first spawn character
        /// </summary>
        /// <param name="ID">ID of character</param>
        public abstract void InitComponent(CharacterID ID, StatData baseStat);
        /// <summary>
        /// Add character stats
        /// </summary>
        public abstract void UpdateStatData();
        /// <summary>
        /// This function runs in update
        /// </summary>
        public abstract void OnUpdateExcute();
    }
}
