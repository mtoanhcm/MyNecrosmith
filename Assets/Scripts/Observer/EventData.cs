using System;
using System.Collections.Generic;
using Building;
using Character;
using Config;
using Equipment;
using Gameplay;
using Inventory.UI;
using Projectile;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Observer {
    public class EventData
    {
        /// <summary>
        /// Open fog of war by radius
        /// </summary>
        public class OpenMinionInventory
        {
            public MinionConfig MinionConfig;
            public Minion.Inventory.Inventory InventoryData;
        }

        public class OnGameplayStateChangeEvent
        {
            public GameplayEventType GameplayState;
            public bool ChangeValue;
        }

        public class OnRequestChangeScene { 
            public SceneManage.SceneType SceneType;
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
            public EquipmentData[] Equipment;
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
            public ProjectileID ID;
            public ProjectileBase Projectile;
        }

        public class OnPlayerInventoryChanged
        {
            public bool HasChange;
        }

        /// <summary>
        /// Call when any minion obtains the equipment or player obtains the equipment from event, box...
        /// </summary>
        public class OnObtainedEquipment
        {
            public EquipmentData EquipmentData;
        }

        /// <summary>
        /// Call when remove equipment from player storage
        /// </summary>
        public class OnRemoveEquipmentFromPlayerStorage
        {
            public EquipmentData EquipmentData;
        }

        public class OnPickEquipmentInInventoryUI
        {
            public EquipmentData Equipment;
        }

        public class OnDraggingInventoryItemUI
        {
            public UIInventoryItem Item;
        }

        public class OnPlaceInventoryItemUI
        {
            public UIInventoryItem UIItem;
            public Action<EquipmentData> OnPlaceItemInMinionInventorySuccess;
        }

        public class OnEquipmentStorageChanged
        {
            public List<EquipmentData> Equipment;
        }

        public class OnSpawnBuilding
        {
            public BuildingID BuildingID;
            public Action<BuildingBase> OnSpawnSuccess;
        }
        
        public class OnDespawnBuilding
        {
            public BuildingBase Building;
        }
    }
}
