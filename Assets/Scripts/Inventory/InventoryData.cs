using System.Collections;
using System.Collections.Generic;
using Equipment;
using Sirenix.Utilities;
using UnityEngine;

namespace Character
{
    public static class InventoryParam
    {
        public static int MIN_ROW = 2;
        public static int MIN_COLUMN = 2;
        public static int MAX_ROW = 9;
        public static int MAX_COLUMN = 9;
        public static int CELL_SIZE = 75;
        public static int MAX_EQUIPMENT_WIDTH = 4;
        public static int MAX_EQUIPMENT_HEIGHT = 4;
        public static int CELL_SPACING = 5;
    }

    public class InventoryItem
    {
        public EquipmentData Equipment { get; private set; }
        public HashSet<(int, int)> PosClaimInventory;
        
        public InventoryItem(EquipmentData equipment)
        {
            Equipment = equipment;
            PosClaimInventory = new HashSet<(int, int)>();
        }

        public void UpdatePosInInventory(HashSet<(int, int)> posClaimInventory)
        {
            PosClaimInventory.Clear();
            PosClaimInventory.AddRange(posClaimInventory);
        }
    }
}
