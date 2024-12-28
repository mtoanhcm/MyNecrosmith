using UnityEngine;

namespace Combat
{
    public class AttackData
    {
        public float Damage { get; }
        public GameObject Attacker { get; }
        public GameObject Target { get; }
        public Vector3 Direction { get; }
        public float AttackRange { get; }
        public float AttackSpeed { get; }
        
        public AttackData(float damage, GameObject attacker, GameObject target, Vector3 direction, float attackRange, float attackSpeed)
        {
            Damage = damage;
            Attacker = attacker;
            Target = target;
            Direction = direction;
            AttackRange = attackRange;
            AttackSpeed = attackSpeed;
        }
    }
}
