using System;
using System.Collections.Generic;
using Character;
using Config;
using Equipment;
using Projectile;
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
            public string MinionID;
            public Action<CharacterBase> OnSpawnSuccess;
        }

        /// <summary>
        /// Call event when minion death
        /// </summary>
        public class OnMinionDeath
        {
            public CharacterBase Minion;
        }

        public class OnPrepareEquipmentForSpawnMinion
        {
            public MinionConfig MinionConfig;
            public List<EquipmentData> Equipment;
        }

        /// <summary>
        /// Call event when the game want to spawn an enemy
        /// </summary>
        public class OnSpawnEnemy
        {
            public string EnemyID;
            public Action<CharacterBase> OnSpawnSuccess;
        }

        /// <summary>
        /// Call event when enemy death
        /// </summary>
        public class OnEnemyDeath
        {
            public CharacterBase Enemy;
        }

        /// <summary>
        /// Call event when player want to spawn a equipment
        /// </summary>
        public class OnSpawnEquipment
        {
            public string EquipmentID;
            public string EquipmentCategoryID;
            public Action<EquipmentBase> OnSpawnEquipmentSuccessHandle;
        }

        /// <summary>
        /// Call event when destroy equipment
        /// </summary>
        public class OnDestroyEquipment
        {
            public EquipmentBase Equipment;
        }

        /// <summary>
        /// Call event when spawn projectile
        /// </summary>
        public class OnSpawnProjectile
        {
            public string ProjectileID;
            public Action<ProjectileBase> OnSpawnSuccess;
        }

        /// <summary>
        /// Call event when despawn projectile
        /// </summary>
        public class OnDespawnProjectile
        {
            public ProjectileBase Projectile;
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
