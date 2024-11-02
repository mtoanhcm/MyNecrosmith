using System.Collections.Generic;
using Character;
using Config;
using Equipment;
using UI;
using UnityEngine;

namespace Observer {
    public class EventData
    {
        /// <summary>
        /// Open fog of war by radius
        /// </summary>
        public struct OpenCharacterInventory
        {
            public Inventory InventoryData;
        }

        public struct DraggingEquipment
        {
            public UIInventoryItem UIItem;
        }

        public struct OnPlacingEquipment
        {
            public UIInventoryItem UIItem;
        }

        public struct OnPickingEquipmentFromInventory
        {
            public UIInventoryItem UIItemPick;
        }

        public struct OnStartGame
        {
            public bool IsStart;
        }

        public struct OnPauseGame
        {
            public bool IsPause;
        }

        public struct OnSpawnMinion
        {
            public CharacterConfig Config;
            public List<EquipmentData> Equipments;
            public Vector3 SpawnPosition;
        }

        public struct OnSpawnEnemy
        {
            public C_Class Class;
            public Vector3 Position;
        }
    }
}
