using Ultility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIInventoryCell : MonoBehaviour
    {
        public int PosX {get; private set;}
        public int PosY {get; private set;}
        public string ItemClaimID {get; private set;}
        public bool IsLocked {get; private set;}
        public bool IsClaimed => !ItemClaimID.IsNulOrEmpty();

        [SerializeField] private Image bg;
        
        
        public void Init(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;

            SetLockCell(true);
        }

        public void SetItemClaim(string itemClaimID)
        {
            ItemClaimID = itemClaimID;
        }

        public void SetLockCell(bool isLocked)
        {
            IsLocked = isLocked;
            
            bg.raycastTarget = !isLocked;
            bg.color = isLocked ? Color.gray : Color.white;
        }

        public void OnHoverOnCell()
        {
            if (IsLocked)
            {
                return;
            }

            bg.color = IsClaimed ? Color.red : Color.green;
        }

        public void OnExitHoverOnCell()
        {
            if (IsLocked)
            {
                return;
            }
            
            bg.color = Color.white;
        }
    }   
}
