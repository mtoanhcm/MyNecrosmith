using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryItemCell : MonoBehaviour
    {
        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }   
}
