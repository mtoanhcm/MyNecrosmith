using System.Collections.Generic;
using Character;
using Config;
using Equipment;
using GameUtility;
using Inventory;
using Observer;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Connects a character with its inventory.
    /// Handles equipment loading, equipping, and inventory management for a character.
    /// </summary>
    public class CharacterInventoryController : MonoBehaviour
    {
        /// <summary>
        /// The character this controller is attached to
        /// </summary>
        [SerializeField] private CharacterBase character;
        
        /// <summary>
        /// The inventory system reference
        /// </summary>
        [SerializeField] private InventorySystem inventorySystem;
        
        /// <summary>
        /// Maximum carry capacity for the character
        /// </summary>
        [SerializeField] private int maxCarryCapacity = 100;
        
        /// <summary>
        /// Dictionary of equipped items by slot
        /// </summary>
        private Dictionary<EquipmentSlotType, EquipmentData> equippedItems;
        
        /// <summary>
        /// Current total weight of inventory items
        /// </summary>
        private int currentWeight;
        
        /// <summary>
        /// Types of equipment slots
        /// </summary>
        public enum EquipmentSlotType
        {
            /// <summary>Head slot for helmets</summary>
            Head,
            
            /// <summary>Body slot for armor</summary>
            Body,
            
            /// <summary>Hands slot for gloves</summary>
            Hands,
            
            /// <summary>Legs slot for boots or leggings</summary>
            Legs,
            
            /// <summary>Primary weapon slot</summary>
            PrimaryWeapon,
            
            /// <summary>Secondary weapon or shield slot</summary>
            SecondaryWeapon
        }
        
        /// <summary>
        /// Initializes the controller
        /// </summary>
        private void Awake()
        {
            equippedItems = new Dictionary<EquipmentSlotType, EquipmentData>();
            currentWeight = 0;
            
            // Register for events
            EventManager.Instance.StartListening<EventData.OnPrepareEquipmentForSpawnMinion>(OnPrepareEquipmentForSpawn);
        }
        
        /// <summary>
        /// Cleans up the controller
        /// </summary>
        private void OnDestroy()
        {
            EventManager.Instance?.StopListening<EventData.OnPrepareEquipmentForSpawnMinion>(OnPrepareEquipmentForSpawn);
        }
        
        /// <summary>
        /// Initializes the inventory for this character
        /// </summary>
        /// <param name="characterId">ID of the character</param>
        public void Initialize(CharacterID characterId)
        {
            // Find or create the character component
            if (character == null)
            {
                character = GetComponent<CharacterBase>();
            }
            
            // Get inventory dimensions from character data
            int rows = InventoryConstants.MAX_ROW;
            int columns = InventoryConstants.MAX_COLUMN;
            
            // If character is a minion, use its specific inventory size
            if (character is MinionCharacter minion && minion.MinionData != null)
            {
                rows = minion.MinionData.InventorySize.x;
                columns = minion.MinionData.InventorySize.y;
            }
            
            // Initialize the inventory system
            if (inventorySystem != null)
            {
                inventorySystem.Initialize(characterId, rows, columns);
                
                // Listen for inventory events
                inventorySystem.OnItemAdded += OnItemAdded;
                inventorySystem.OnItemRemoved += OnItemRemoved;
            }
        }
        
        /// <summary>
        /// Shows the character's inventory
        /// </summary>
        public void ShowInventory()
        {
            if (inventorySystem != null)
            {
                inventorySystem.Show();
            }
            else
            {
                // Create an event to open inventory
                EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory
                {
                    CharacterID = character.Data.ID,
                    InventoryData = new InventoryData(
                        InventoryConstants.MAX_ROW,
                        InventoryConstants.MAX_COLUMN,
                        character.Data.ID)
                });
            }
        }
        
        /// <summary>
        /// Hides the character's inventory
        /// </summary>
        public void HideInventory()
        {
            if (inventorySystem != null)
            {
                inventorySystem.Hide();
            }
            else
            {
                // Create an event to close inventory
                EventManager.Instance.TriggerEvent(new EventData.OpenCharacterInventory
                {
                    InventoryData = null
                });
            }
        }
        
        /// <summary>
        /// Equips an item in a specific slot
        /// </summary>
        /// <param name="itemId">ID of the item to equip</param>
        /// <param name="slot">Slot to equip the item in</param>
        /// <returns>True if the item was equipped, false otherwise</returns>
        public bool EquipItem(EquipmentID itemId, EquipmentSlotType slot)
        {
            // Get the item from the inventory
            InventoryItem item = inventorySystem?.GetItem(itemId);
            
            if (item == null)
                return false;
                
            // Check if the item can be equipped in this slot
            if (!CanEquipInSlot(item.Equipment, slot))
                return false;
                
            // Unequip any existing item in this slot
            if (equippedItems.TryGetValue(slot, out var existingItem))
            {
                UnequipItem(slot);
            }
            
            // Equip the new item
            equippedItems[slot] = item.Equipment;
            
            // Remove from inventory
            inventorySystem?.RemoveItem(itemId);
            
            // Apply equipment stats
            ApplyEquipmentStats();
            
            return true;
        }
        
        /// <summary>
        /// Unequips an item from a slot
        /// </summary>
        /// <param name="slot">The slot to unequip</param>
        /// <returns>The unequipped item, or null if the slot was empty</returns>
        public EquipmentData UnequipItem(EquipmentSlotType slot)
        {
            if (!equippedItems.TryGetValue(slot, out var item))
                return null;
                
            // Remove from equipped items
            equippedItems.Remove(slot);
            
            // Add back to inventory
            inventorySystem?.AddItem(item);
            
            // Apply equipment stats
            ApplyEquipmentStats();
            
            return item;
        }
        
        /// <summary>
        /// Checks if an item can be equipped in a specific slot
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="slot">The slot to check</param>
        /// <returns>True if the item can be equipped in the slot, false otherwise</returns>
        public bool CanEquipInSlot(EquipmentData item, EquipmentSlotType slot)
        {
            // Check if the item is compatible with the slot
            switch (slot)
            {
                case EquipmentSlotType.Head:
                case EquipmentSlotType.Body:
                case EquipmentSlotType.Hands:
                case EquipmentSlotType.Legs:
                    return item.CategoryID == EquipmentCategoryID.Armor;
                    
                case EquipmentSlotType.PrimaryWeapon:
                case EquipmentSlotType.SecondaryWeapon:
                    return item.CategoryID.IsWeaponType();
                    
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Checks if the character can carry an additional item
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True if the character can carry the item, false otherwise</returns>
        public bool CanCarryItem(EquipmentData item)
        {
            if (item == null)
                return false;
                
            return currentWeight + item.LoadPoint <= maxCarryCapacity;
        }
        
        /// <summary>
        /// Gets all equipment currently equipped on the character
        /// </summary>
        /// <returns>List of equipped equipment</returns>
        public List<EquipmentData> GetEquippedEquipment()
        {
            List<EquipmentData> equipment = new List<EquipmentData>();
            
            foreach (var item in equippedItems.Values)
            {
                equipment.Add(item);
            }
            
            return equipment;
        }
        
        /// <summary>
        /// Applies equipment stats to the character
        /// </summary>
        private void ApplyEquipmentStats()
        {
            // Clear existing stats
            // TODO: Reset character stats to base values
            
            // Apply stats from each equipped item
            foreach (var item in equippedItems.Values)
            {
                ApplyItemStats(item);
            }
            
            // TODO: Update character visuals to reflect equipped items
        }
        
        /// <summary>
        /// Applies stats from a specific item to the character
        /// </summary>
        /// <param name="item">The item to apply</param>
        private void ApplyItemStats(EquipmentData item)
        {
            // Apply specific stats based on item type
            if (item is ArmorData armorData)
            {
                // Apply armor stats
                // TODO: Apply armor-specific stats
            }
            else if (item is WeaponData weaponData)
            {
                // Apply weapon stats
                // TODO: Apply weapon-specific stats
            }
            
            // Apply generic stats
            // TODO: Apply generic stats
        }
        
        /// <summary>
        /// Handles item added to inventory
        /// </summary>
        /// <param name="item">The added item</param>
        private void OnItemAdded(EquipmentData item)
        {
            currentWeight += item.LoadPoint;
        }
        
        /// <summary>
        /// Handles item removed from inventory
        /// </summary>
        /// <param name="itemId">ID of the removed item</param>
        private void OnItemRemoved(EquipmentID itemId)
        {
            // Find the removed item in the inventory
            InventoryItem item = inventorySystem?.GetItem(itemId);
            
            if (item != null)
            {
                currentWeight -= item.Equipment.LoadPoint;
            }
        }
        
        /// <summary>
        /// Handles preparation of equipment for character spawning
        /// </summary>
        /// <param name="data">Event data</param>
        private void OnPrepareEquipmentForSpawn(EventData.OnPrepareEquipmentForSpawnMinion data)
        {
            if (character != null && character is MinionCharacter minion)
            {
                minion.InitEquipment(data.Equipment);
            }
        }
        
        /// <summary>
        /// Gets the current inventory weight
        /// </summary>
        public int CurrentWeight => currentWeight;
        
        /// <summary>
        /// Gets the maximum carry capacity
        /// </summary>
        public int MaxCarryCapacity => maxCarryCapacity;
    }
}