using UnityEngine;

namespace UI
{
    public class UIItemDragCell : MonoBehaviour
    {
        public RectTransform RectTrans => rectTrans;
        private RectTransform rectTrans;

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
        }
    }   
}
