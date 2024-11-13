using System.Collections;
using System.Collections.Generic;
using Equipment;
using UnityEngine;

namespace Character
{
    public class Inventory
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        
        public C_Class CharacterClass { get; private set; }
        public List<InventoryItem> Items { get; private set; }

        public Inventory(int row, int column, C_Class characterClass)
        {
            Row = row;
            Column = column;
            
            Items = new List<InventoryItem>();
            CharacterClass = characterClass;
        }

        public EquipmentData[] GetEquipmentData()
        {
            var totalItem = Items.Count;
            var tempEquipmentLst = new EquipmentData[totalItem];
            
            if (totalItem == 0)
            {
                return tempEquipmentLst;
            }

            for (var i = 0; i < totalItem; i++)
            {
                tempEquipmentLst[i] = Items[i].Equipment;
            }
            
            return tempEquipmentLst;
        }
    }   
}
