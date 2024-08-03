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
            InitComponent();
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

        public abstract void InitComponent();
        public abstract void OnUpdateExcute();
    }
}
