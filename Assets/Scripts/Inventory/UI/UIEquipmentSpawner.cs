using System;
using GameUtility;
using UnityEngine;

namespace  UI
{
    public class UIEquipmentSpawner : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem equipmentUIItemPrefab;
        
        private GameObjectPool<UIInventoryItem> equipmentUIItemPool;

        public void Init()
        {
            equipmentUIItemPool =
                new GameObjectPool<UIInventoryItem>(OnCreateObj, OnGetObjFromPool, OnReturnObjToPool, transform, 5);
        }

        public UIInventoryItem GetEquipmentUIItem()
        {
            return equipmentUIItemPool.GetObject("EquipmentUIItem");
        }

        public void ReturnEquipmentUIItem(UIInventoryItem equipmentUIItem)
        {
            equipmentUIItemPool.ReturnToPool("EquipmentUIItem", equipmentUIItem);
        }
        
        private UIInventoryItem OnCreateObj(string arg)
        {
            var item = Instantiate(equipmentUIItemPrefab, transform);
            item.SetActive(false);
            
            return item;
        }
        
        void OnGetObjFromPool(UIInventoryItem obj)
        {
            obj.SetActive(true);
        }
        
        void OnReturnObjToPool(UIInventoryItem obj)
        {
            obj.SetActive(false);
        }
    }   
}
