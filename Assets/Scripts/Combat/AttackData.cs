using Config;
using Projectile;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class AttackData
    {
        public ProjectileDataSO ProjectileConfig { get; set; }
        public float Damage { get; set; }
        public GameObject Attacker { get; set; }
        public GameObject Target { get; set; }
        public Vector3 SpawnPos { get; set; }
        public Vector3 Direction { get; set; }
        public float AttackRange { get; set; }
        public float AttackSpeed { get; set; }
        public UnityAction OnDestroyTarget { get; set; }
    }
}
