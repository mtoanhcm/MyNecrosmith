using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Component {
    public class StatComponent
    {
        public CharacterID ID { get; set; }
        public int Level { get; set; }
        public float CurHP { get; set; }
        public float MaxHP { get; set; }
        public float Speed { get; set; }
        public float AttackSpeed { get; set; }
        public float ScanRange { get; set; }
        public float AttackRange { get; set; }
        public float Damage { get; set; }

        private StatData baseData;

        public StatComponent(int level, CharacterID id, StatData baseData)
        {
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
