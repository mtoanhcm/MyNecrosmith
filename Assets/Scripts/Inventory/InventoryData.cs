using System.Collections;
using System.Collections.Generic;
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
        public int PosX;
        public int PosY;

        public InventoryItem(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }
    }
}
