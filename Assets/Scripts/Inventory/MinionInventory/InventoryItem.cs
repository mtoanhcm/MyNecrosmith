using System.Collections;
using System.Collections.Generic;
using Equipment;
using Sirenix.Utilities;
using UnityEngine;

namespace Minion.Inventory
{
    public class InventoryItem
    {
        public int Index { get; private set; }
        public EquipmentData Equipment { get; private set; }
        public HashSet<(int, int)> PosClaimInventory { get; private set; }
        
        public InventoryItem(EquipmentData equipment)
        {
            Equipment = equipment;
            PosClaimInventory = new HashSet<(int, int)>();
        }

        public void UpdatePosInInventory(int index ,HashSet<(int, int)> posClaimInventory)
        {
            PosClaimInventory.Clear();
            PosClaimInventory.AddRange(posClaimInventory);
            Index = index;
        }
    }
}
