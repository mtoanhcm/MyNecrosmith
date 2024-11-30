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
            public CharacterClass CharacterClass;
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
            public MinionConfig Config;
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

        /// <summary>
        /// Call when any minion obtains the equipment or player obtains the equipment from event, box...
        /// </summary>
        public struct OnObtainedEquipment
        {
            public EquipmentData EquipmentData;
        }

        public struct OnChooseEquipmentInStorage
        {
            public EquipmentData Equipment;
        }
    }
}
