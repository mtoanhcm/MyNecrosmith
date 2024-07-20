using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void Destroy() { }
    }
}
