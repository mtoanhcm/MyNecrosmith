using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building {
    public abstract class BuildingBase : MonoBehaviour
    {
        protected BuildingData data;

        public abstract void Spawn();
        public abstract void Claimp();
        public abstract void TakeDamage(float damage);
        
    }
}
