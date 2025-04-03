using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUtility.UI
{
    public class UIDragHandle<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler where T : Component
    {
        public Action<T> OnPickItemAction;
        public Action<T> OnDraggingItemAction;
        public Action<T> OnReleaseItemAction;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPickItemAction?.Invoke(this as T);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleaseItemAction?.Invoke(this as T);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDraggingItemAction?.Invoke(this as T);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }   
}
