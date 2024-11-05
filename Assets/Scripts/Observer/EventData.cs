using System;
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
            public C_Class CharacterClass;
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

        /// <summary>
        /// Call event when player want to spawn a minion
        /// </summary>
        public struct OnSpawnMinion
        {
            public CharacterConfig Config;
            public List<EquipmentData> Equipments;
            public Vector3 SpawnPosition;
        }

        /// <summary>
        /// Call event when player want to spawn a equipment
        /// </summary>
        public struct OnSpawnEquipment
        {
            public EquipmentData Equipment;
            public CharacterBase Owner;
            public Vector3 SpawnPosition;
            public Action<EquipmentBase> OnSpawnEqupimentSuccessHandle;
        }

        public struct OnMinionEquipmentReady
        {
            public C_Class CharacterClass;
            public List<EquipmentData> Equipments;
        }

        public struct OnSpawnEnemy
        {
            public C_Class Class;
            public Vector3 Position;
        }

        public struct OnLoadCharacterPrefabSuccess
        {
            public string Class;
            public CharacterBase CharPrefab;
        }
        
        public struct OnLoadEquipmentPrefabSuccess
        {
            public string EquipmentTypeID;
            public EquipmentBase EquipmentPrefab;
        }
    }
}
