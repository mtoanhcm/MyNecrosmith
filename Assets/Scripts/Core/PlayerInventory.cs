using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Equipment;
using Observer;
using Sirenix.Serialization;
using UnityEngine;

namespace Gameplay
{
    public class PlayerInventory : MonoBehaviour
    {
        public readonly struct EquipmentKey : IEquatable<EquipmentKey>
        {
            public readonly int Level;
            public readonly Rarity Rarity;
            public readonly EquipmentID ID;
            public readonly EquipmentCategoryID Category;
            
            public EquipmentKey(int level, Rarity rarity, EquipmentID id, EquipmentCategoryID category)
            {
                Level = level;
                Rarity = rarity;
                ID = id;
                Category = category;
            }
            
            public override bool Equals(object obj)
            {
                return obj is EquipmentKey key && Equals(key);
            }
            
            public bool Equals(EquipmentKey other)
            {
                return Level == other.Level && 
                       Rarity == other.Rarity && 
                       ID == other.ID;
            }
            
            public override int GetHashCode()
            {
                return HashCode.Combine(Level, Rarity, ID);
            }
        }
        
        public Dictionary<EquipmentKey, List<EquipmentData>> PlayerEquipments => playerEquipments;
        
        private Dictionary<EquipmentKey, List<EquipmentData>> playerEquipments;

        public void Init()
        {
            playerEquipments = new Dictionary<EquipmentKey, List<EquipmentData>>();
        }

        public void Clear()
        {
            playerEquipments.Clear();
        }
        
        public void AddEquipmentToStorage(EquipmentData data)
        {
            var key = new EquipmentKey(data.Level, data.Rarity, data.EquipmentID, data.CategoryID);
            
            if (!playerEquipments.ContainsKey(key))
            {
                playerEquipments[key] = new List<EquipmentData>();
            }
            
            playerEquipments[key].Add(data);
        }

        public bool RemoveEquipment(EquipmentData equipment)
        {
            if (equipment == null) return false;

            var key = new EquipmentKey(equipment.Level, equipment.Rarity, equipment.EquipmentID, equipment.CategoryID);
        
            if (playerEquipments.TryGetValue(key, out var equipments))
            {
                var removed = equipments.Remove(equipment);
            
                if (equipments.Count == 0)
                {
                    playerEquipments.Remove(key);
                }
            
                return removed;
            }
        
            return false;
        }
        
        public List<EquipmentData> GetEquipments(int level, Rarity rarity, EquipmentID id, EquipmentCategoryID category)
        {
            var key = new EquipmentKey(level, rarity, id, category);
        
            if (playerEquipments.TryGetValue(key, out var equipments))
            {
                return equipments;
            }
        
            return new List<EquipmentData>();
        }
        
        public List<EquipmentData> GetEquipments(int level)
        {
            return playerEquipments
                .Where(g => g.Key.Level == level)
                .SelectMany(g => g.Value)
                .ToList();
        }
        
        public List<EquipmentData> GetEquipments(Rarity rarity)
        {
            return playerEquipments
                .Where(g => g.Key.Rarity == rarity)
                .SelectMany(g => g.Value)
                .ToList();
        }
        
        public List<EquipmentData> GetEquipments(EquipmentID id)
        {
            return playerEquipments
                .Where(g => g.Key.ID == id)
                .SelectMany(g => g.Value)
                .ToList();
        }
        
        public int GetTotalEquipmentCount()
        {
            return playerEquipments.Values.Sum(list => list.Count);
        }
        
        public Dictionary<EquipmentKey, List<EquipmentData>> GetEquipmentsByCategory(EquipmentCategoryID category)
        {
            Dictionary<EquipmentKey, List<EquipmentData>> filteredEquipments = new Dictionary<EquipmentKey, List<EquipmentData>>();
    
            foreach (var equipment in playerEquipments)
            {
                if (category != EquipmentCategoryID.None && equipment.Key.Category != category)
                {
                    continue;
                }
                
                filteredEquipments[equipment.Key] = equipment.Value;
            }
    
            return filteredEquipments;
        }
    }   
}
