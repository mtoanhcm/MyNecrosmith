using GameUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Minion.Inventory.UI
{
    public class UIMinionInventoryCell : MonoBehaviour
    {
        public int PosX {get; private set;}
        public int PosY {get; private set;}
        public string ItemClaimID {get; private set;}
        public bool IsLocked {get; private set;}
        public bool IsClaimed => !ItemClaimID.IsNulOrEmpty();

        [SerializeField] private Image bgImg;


        public void Init(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;

            SetLockCell(true);
        }
        
        public void SetLockCell(bool isLocked)
        {
            IsLocked = isLocked;
    
            bgImg.raycastTarget = !isLocked;
            bgImg.color = isLocked ? Color.gray : Color.white;
        }

        public void OnHoverOnCell()
        {
            if (IsLocked)
            {
                return;
            }

            bgImg.color = IsClaimed ? Color.red : Color.green;
        }
        
        public void OnExitHoverOnCell()
        {
            if (IsLocked)
            {
                return;
            }
    
            bgImg.color = Color.white;
        }
        
        public void SetItemClaim(string itemClaimID)
        {
            ItemClaimID = itemClaimID;
        }
    }   
}
