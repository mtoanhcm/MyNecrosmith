using Character;
using UnityEngine;

namespace Pool
{
    public abstract class PoolBase<T> : MonoBehaviour where T : Component
    {
        protected ObjectPool<T> pool;

        private void Awake()
        {
            InitPool();
        }
        
        
        protected virtual void InitPool()
        {
            pool = new ObjectPool<T>(transform, 5, 200);
        }
    }
}
