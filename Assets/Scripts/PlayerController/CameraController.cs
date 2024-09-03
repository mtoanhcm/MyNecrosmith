using Map;
using Observer;
using UnityEngine;

namespace PlayerController {
    public class CameraController : MonoBehaviour
    {
        [Header("----- Move Camera -----")]
        public float panSpeed = 5f;
        public float edgeBoundary = 10f; // Khoảng cách từ cạnh màn hình để bắt đầu di chuyển

        [Header("----- Zoom Camera -----")]
        public float zoomSpeed = 10f;    // Tốc độ zoom
        public float minZoom = 5f;       // Giới hạn zoom in
        public float maxZoom = 20f;      // Giới hạn zoom out

        private FogManager fogOfWar;
        private Bounds fogOfWarBound;
        private bool isFogVisible;

        private void Start()
        {
            fogOfWar = MapManager.Instance.fogManager;
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening<EventData.OpenFogWarSuccessEvent>(CheckExpandBounds);
        }

        private void LateUpdate()
        {
            if (!isFogVisible) {
                return;
            }

            PanCamera();
            ZoomCamera();
        }

        private void CheckExpandBounds(EventData.OpenFogWarSuccessEvent data) {
            if (!data.IsOpenFogSuccess) {
                return;
            }

            fogOfWarBound = fogOfWar.GetClearAreaBounds();
            isFogVisible = true;
        }

        void PanCamera()
        {
            Vector3 move = Vector3.zero;

            if (Input.mousePosition.x >= Screen.width - edgeBoundary)
            {
                move.x += panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x <= edgeBoundary)
            {
                move.x -= panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y >= Screen.height - edgeBoundary)
            {
                move.y += panSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y <= edgeBoundary)
            {
                move.y -= panSpeed * Time.deltaTime;
            }

            Vector3 newPosition = Camera.main.transform.position + move;

            // Giới hạn di chuyển của camera trong ranh giới thu hẹp
            newPosition.x = Mathf.Clamp(newPosition.x, fogOfWarBound.min.x, fogOfWarBound.max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, fogOfWarBound.min.y, fogOfWarBound.max.y);

            Camera.main.transform.position = newPosition;
        }

        void ZoomCamera()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float newSize = Camera.main.orthographicSize - scroll * zoomSpeed;

            // Giới hạn zoom in/out
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

            Camera.main.orthographicSize = newSize;
        }
    }
}
