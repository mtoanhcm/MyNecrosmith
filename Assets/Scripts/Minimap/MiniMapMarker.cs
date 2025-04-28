using Observer;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMap {
    public class MiniMapMarker : MonoBehaviour
    {
        public RectTransform markerUI;
        private MiniMapController miniMap;

        private void OnEnable()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnAddMiniMapMarker() { Marker = this });
        }

        private void OnDisable()
        {
            EventManager.Instance.TriggerEvent(new EventData.OnRemoveMiniMapMarker() { Marker = this });
        }

        public void UpdateMarkerPosition(Vector2 position, RectTransform miniMapRect)
        {
            if (markerUI != null)
            {
                markerUI.anchoredPosition = position - new Vector2(miniMapRect.rect.width / 2, miniMapRect.rect.height / 2);
            }
        }
    }
}
