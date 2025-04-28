using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Observer;

namespace MiniMap
{
    public class MiniMapController : MonoBehaviour, IPointerClickHandler
    {
        [Header("Mini-Map Settings")]
        public RectTransform miniMapRect;
        public RawImage miniMapImage;
        public float minZoom = 0.5f;
        public float maxZoom = 2f;
        public float zoomSpeed = 0.1f;

        [Header("World Settings")]
        public Vector2 worldSize = new Vector2(500, 500); // World size in X and Z

        private List<MiniMapMarker> markers = new List<MiniMapMarker>();
        private float currentZoom = 1f;

        private bool isInit;

        private void Start()
        {
            if (miniMapImage.texture == null)
            {
                Debug.LogError("MiniMapController: Missing mini-map image or texture!");
                return;
            }

            EventManager.Instance.StartListening<EventData.OnAddMiniMapMarker>(OnAddMarker);
            EventManager.Instance.StartListening<EventData.OnRemoveMiniMapMarker>(OnRemoveMarker);

            isInit = true;
        }

        void Update()
        {
            UpdateMarkers();

            // Zoom control (mouse wheel for example)
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                Zoom(scroll);
            }
        }

        public void RegisterMarker(MiniMapMarker marker)
        {
            if (!markers.Contains(marker))
                markers.Add(marker);
        }

        public void UnregisterMarker(MiniMapMarker marker)
        {
            if (markers.Contains(marker))
                markers.Remove(marker);
        }

        private void OnAddMarker(EventData.OnAddMiniMapMarker data)
        {
            if (data.Marker == null)
            {
                return;
            }

            RegisterMarker(data.Marker);
        }

        private void OnRemoveMarker(EventData.OnRemoveMiniMapMarker data)
        {
            if (data.Marker == null)
            {
                return;
            }

            UnregisterMarker(data.Marker);
        }

        private void UpdateMarkers()
        {
            foreach (MiniMapMarker marker in markers)
            {
                if (marker == null) continue;

                Vector2 miniMapPos = WorldToMiniMap(marker.transform.position);
                marker.UpdateMarkerPosition(miniMapPos, miniMapRect);
            }
        }

        Vector2 WorldToMiniMap(Vector3 worldPos)
        {
            float normalizedX = worldPos.x / worldSize.x;
            float normalizedY = worldPos.z / worldSize.y;

            float mapWidth = miniMapRect.rect.width;
            float mapHeight = miniMapRect.rect.height;

            return new Vector2(normalizedX * mapWidth, normalizedY * mapHeight);
        }

        void Zoom(float scrollDelta)
        {
            currentZoom = Mathf.Clamp(currentZoom + scrollDelta * zoomSpeed, minZoom, maxZoom);
            miniMapRect.localScale = Vector3.one * currentZoom;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(miniMapRect, eventData.position, eventData.pressEventCamera, out localPoint);

            Vector2 normalizedPoint = new Vector2(
                (localPoint.x + miniMapRect.rect.width / 2) / miniMapRect.rect.width,
                (localPoint.y + miniMapRect.rect.height / 2) / miniMapRect.rect.height
            );

            Vector3 worldPos = new Vector3(
                normalizedPoint.x * worldSize.x,
                0f, // Assuming Y is 0 in world
                normalizedPoint.y * worldSize.y
            );

            Debug.Log("MiniMap clicked at world position: " + worldPos);

            // You can call an event or method here to use worldPos
        }
    }
}
