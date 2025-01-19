using System;
using Character;
using Pool;
using UnityEngine;

namespace Spawner
{
    public class EnemyObjectPool : MonoBehaviour
    {
        private ObjectPool<CharacterBase> pool;

        private void Awake()
        {
            pool = new ObjectPool<CharacterBase>(transform, 5, 200);
        }
    }   
}
