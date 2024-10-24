using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Inventory
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        
        public List<InventoryItem> Items { get; private set; }

        public Inventory(int row, int column)
        {
            Row = row;
            Column = column;
            
            Items = new List<InventoryItem>();
        }
    }   
}
