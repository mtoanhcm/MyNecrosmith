using System.Threading.Tasks;
using UnityEngine;

namespace Pool
{
    public class PoolBase<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected int initialSize = 5;
        [SerializeField] protected int maxSize = 200;
    
        private ObjectPool<T> pool;

        private void Awake()
        {
            pool = new ObjectPool<T>(transform, initialSize, maxSize);
        }

        private void OnDestroy()
        {
            pool?.Dispose();
        }

        /// <summary>
        /// Get object from pool with specific ID
        /// </summary>
        /// <param name="itemID">item id need to get</param>
        /// <returns>Gameobject from pool</returns>
        public async Task<T> GetObject(string itemID)
        {
            return await pool.Get(itemID);
        }

        /// <summary>
        /// Return object back to pool
        /// </summary>
        /// <param name="itemID">Item ID need to return</param>
        /// <param name="item">Item need to return</param>
        public void ReturnObject(string itemID, T item)
        {
            pool.Return(itemID, item);
        }
    }
}
