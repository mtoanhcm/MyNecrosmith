using Character.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Character {
    public abstract class CharacterBase : MonoBehaviour
    {
        protected StatComponent statComp;

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
        /// <summary>
        /// Deal damage to character
        /// </summary>
        /// <param name="damage">Damage amount</param>
        public virtual void TakeDamage(float damage) {
            statComp.CurHP -= Mathf.Max(0, damage);
            if (statComp.CurHP <= 0)
            {
                Death();
            }
        }
        /// <summary>
        /// Restore character HP
        /// </summary>
        /// <param name="amount">Hp restore amount</param>
        public virtual void RestoreHP(float amount){
            statComp.CurHP += Mathf.Max(0, amount);
            statComp.CurHP = Mathf.Min(statComp.CurHP, statComp.MaxHP);
        }

        /// <summary>
        /// Init character component, need to call when first spawn character
        /// </summary>
        /// <param name="ID">ID of character</param>
        public abstract void Spawn(CharacterID ID, Vector3 spawnPos, StatData baseStat);
        /// <summary>
        /// Add character stats
        /// </summary>
        public abstract void UpdateStatData();
        /// <summary>
        /// This function runs in update
        /// </summary>
        public abstract void OnUpdateExcute();
        public abstract void Death();
    }
}
