using UnityEngine;

namespace UI
{
    public class UIItemDragCell : MonoBehaviour
    {
        public RectTransform RectTrans => rectTrans;
        private RectTransform rectTrans;
        public bool IsVisible => gameObject.activeSelf;

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }   
}
