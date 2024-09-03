using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Component {
    public class StatComponent
    {
        public CharacterID ID { get; private set; }
        public int Level { get; private set; }
        public float CurHP { get; private set; }
        public float MaxHP { get; private set; }
        public float Speed { get; private set; }
        public float AttackSpeed { get; private set; }
        public float ScanRange { get; private set; }
        public float AttackRange { get; private set; }
        public float Damage { get; private set; }

        private StatData baseData;

        public StatComponent(int level, CharacterID id, StatData baseData) {
            Level = level;
            ID = id;

            MaxHP = baseData.HP;
            CurHP = baseData.HP;
            Damage = baseData.Damage;
            Speed = baseData.Speed;
            AttackSpeed = baseData.AttackSpeed;
            ScanRange = baseData.ScanRange;
            AttackRange = baseData.AttackRange;
        }
    }
}
