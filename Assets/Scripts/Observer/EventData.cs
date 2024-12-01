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
        public class OpenCharacterInventory
        {
            public CharacterID CharacterID;
            public Inventory InventoryData;
        }

        public class DraggingEquipment
        {
            public UIInventoryItem UIItem;
        }

        public class OnPlacingEquipment
        {
            public UIInventoryItem UIItem;
        }

        public class OnPickingEquipmentFromInventory
        {
            public UIInventoryItem UIItemPick;
        }

        public class OnStartGame
        {
            public bool IsStart;
        }

        public class OnPauseGame
        {
            public bool IsPause;
        }

        /// <summary>
        /// Call event when player want to spawn a minion
        /// </summary>
        public class OnSpawnMinion
        {
            public MinionConfig Config;
            public List<EquipmentData> Equipments;
            public Vector3 SpawnPosition;
        }

        /// <summary>
        /// Call event when player want to spawn a equipment
        /// </summary>
        public class OnSpawnEquipment
        {
            public EquipmentData Equipment;
            public CharacterBase Owner;
            public Vector3 SpawnPosition;
            public Action<EquipmentBase> OnSpawnEqupimentSuccessHandle;
        }

        public class OnLoadCharacterPrefabSuccess
        {
            public string Class;
            public CharacterBase CharPrefab;
        }
        
        public class OnLoadEquipmentPrefabSuccess
        {
            public string EquipmentTypeID;
            public EquipmentBase EquipmentPrefab;
        }

        /// <summary>
        /// Call when any minion obtains the equipment or player obtains the equipment from event, box...
        /// </summary>
        public class OnObtainedEquipment
        {
            public EquipmentData EquipmentData;
        }

        public class OnChooseEquipmentInStorage
        {
            public EquipmentData Equipment;
        }
    }
}
