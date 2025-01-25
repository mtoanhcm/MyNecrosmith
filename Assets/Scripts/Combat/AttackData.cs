using Config;
using Projectile;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class AttackData
    {
        public ProjectileDataSO ProjectileConfig { get; }
        public float Damage { get; }
        public GameObject Attacker { get; }
        public GameObject Target { get; }
        public Vector3 SpawnPos { get; }
        public Vector3 Direction { get; }
        public float AttackRange { get; }
        public float AttackSpeed { get; }
        public UnityAction OnDestroyTarget;
    }
}
